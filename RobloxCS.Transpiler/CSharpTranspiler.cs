using System.Diagnostics;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using RobloxCS.AST;
using RobloxCS.AST.Expressions;
using RobloxCS.AST.Functions;
using RobloxCS.AST.Parameters;
using RobloxCS.AST.Prefixes;
using RobloxCS.AST.Statements;
using RobloxCS.AST.Suffixes;
using RobloxCS.AST.Types;
using RobloxCS.Transpiler.Scoping;
using Serilog;
using FunctionCall = RobloxCS.AST.Expressions.FunctionCall;
using TypeInfo = RobloxCS.AST.Types.TypeInfo;

namespace RobloxCS.Transpiler;

/// <summary>
/// The big momma of RobloxCS
/// </summary>
public sealed class CSharpTranspiler : CSharpSyntaxWalker {
    public TranspilerOptions Options { get; }
    public CSharpCompiler Compiler { get; }
    public SemanticModel Semantics { get; }
    public CompilationUnitSyntax Root { get; }

    public List<Statement> Exports = [];

    private readonly Stack<Block> _blockStack = new();

    private Block CurrentBlock => _blockStack.Peek();
    private TypeDeclaration CurrentType { get; set; } = NoTypeSentinel;

    private static readonly Block RootBlock = Block.Empty();
    private static readonly TypeDeclaration NoTypeSentinel = TypeDeclaration.EmptyTable("__SHOULD_NOT_BE_IN_OUTPUT");

    public CSharpTranspiler(TranspilerOptions options, CSharpCompiler compiler) {
        Options = options;
        Compiler = compiler;

        Root = Compiler.Root;
        Semantics = Compiler.Compilation.GetSemanticModel(Root.SyntaxTree);

        _blockStack.Push(RootBlock); // seeded stack so we can avoid null checks
    }

    public Chunk Transpile() {
        Log.Information("Starting to transpile");

        var watch = Stopwatch.StartNew();

        Visit(Root);
        var chunk = new Chunk { Block = RootBlock };

        watch.Stop();

        Log.Information("Finished transpiling in {TimeMS}ms", watch.ElapsedMilliseconds);

        return chunk;
    }

    public override void VisitClassDeclaration(ClassDeclarationSyntax node) {
        var classSymbol = Semantics.CheckedGetDeclaredSymbol(node);
        var className = classSymbol.Name;

        Log.Verbose("Visiting class declaration {ClassName}", className);

        var (instanceDecl, typeDecl, ctorField, newCtorField) = BuildClassTypeDeclarations(node);
        CurrentBlock.AddStatement(instanceDecl);
        CurrentBlock.AddStatement(typeDecl);

        var newField = ctorField.DeepClone();
        if (typeDecl.DeclareAs is TableTypeInfo typeTable) {
            newField.Key = NameTypeFieldKey.FromString("new");
            if (newField.Value is CallbackTypeInfo { Arguments.Count: > 0 } cb) {
                cb.Arguments.RemoveAt(0); // remove self
                cb.ReturnType = BasicTypeInfo.FromString($"_Type{className}");
            }

            typeTable.Fields.Add(newField);
            Log.Verbose("Created constructor field");

            var index = TypeField.FromNameAndType("__index", BasicTypeInfo.FromString(typeDecl.Name));
            typeTable.Fields.Add(index);
            Log.Verbose("Created __index field");

            var tostring = TypeField.FromNameAndType("__tostring", new CallbackTypeInfo { Arguments = [], ReturnType = BasicTypeInfo.String() });
            typeTable.Fields.Add(tostring);
            Log.Verbose("Created __tostring field");
        }

        var both = IntersectionTypeInfo.FromNames(instanceDecl.Name, typeDecl.Name);
        CurrentBlock.AddStatement(LocalAssignment.Naked(node.Identifier.ValueText, both));

        var classBlock = Block.Empty();
        using (WithBlock(classBlock, $"ClassBlock_{className}")) {
            Log.Debug("Creating required functions for class functionality");

            Log.Verbose("Creating __tostring body for {ClassName}", className);
            var toStringBlock = Block.Empty();
            using (WithBlock(toStringBlock, $"FunctionToStringBlock_{className}")) {
                CurrentBlock.AddStatement(Return.FromExpressions([StringExpression.FromString(className)]));
            }

            var toStringFunction = new AnonymousFunction {
                Body = new FunctionBody {
                    Body = toStringBlock,
                    Parameters = [],
                    ReturnType = BasicTypeInfo.String(),
                    TypeSpecifiers = [],
                },
            };

            CurrentBlock.AddStatement(new Assignment {
                Vars = [VarName.FromString(className)],
                Expressions = [
                    FunctionCall.Basic("setmetatable", TableConstructor.Empty(), TableConstructor.With(new NameKey { Key = "__tostring", Value = toStringFunction })),
                ],
            });

            CurrentBlock.AddStatement(Assignment.AssignToSymbol($"{className}.__index", className));

            foreach (var newNode in CreateNewMethods(classSymbol)) {
                CurrentBlock.AddStatement(newNode);
            }

            foreach (var ctorNode in CreateConstructors(classSymbol)) {
                CurrentBlock.AddStatement(ctorNode);
            }
        }

        CurrentBlock.AddStatement(DoStatement.FromBlock(classBlock));
    }

    public override void VisitFieldDeclaration(FieldDeclarationSyntax node) {
        if (WithTableFields(table => {
            foreach (var f in GenerateTypeFieldsFromField(node)) table.Fields.Add(f);
        })) {
            return; // we are generating a type, return
        }

        Console.WriteLine("Hey");

        foreach (var decl in node.Declaration.Variables) {
            Log.Warning("Unimplemented initializer for field {FieldName}", decl.Identifier.ValueText);
        }
    }

    private IEnumerable<Statement> CreateNewMethods(INamedTypeSymbol classSymbol) {
        foreach (var ctor in classSymbol.InstanceConstructors) {
            Log.Debug("Found constructor for {ClassName}", classSymbol.Name);

            if (ctor.IsImplicitlyDeclared) {
                // always parameterless
                yield return CreateParameterlessNewMethod(classSymbol);
            } else {
                yield return CreateUserDefinedConstructor(classSymbol, ctor);
            }
        }
    }

    private FunctionDeclaration CreateParameterlessNewMethod(INamedTypeSymbol classSymbol) {
        var functionBlock = CreateNewMethodBlock(classSymbol);
        var functionNode = new FunctionDeclaration {
            Name = FunctionName.FromString($"{classSymbol.Name}.new"),
            Body = new FunctionBody {
                Parameters = [],
                TypeSpecifiers = [],
                Body = functionBlock,
                ReturnType = BasicTypeInfo.FromString($"_Instance{classSymbol.Name}"),
            },
        };

        return functionNode;
    }

    private Block CreateNewMethodBlock(INamedTypeSymbol classSymbol) {
        var block = Block.Empty();
        using (WithBlock(block, $"FunctionBlock_New{classSymbol.Name}")) {
            CurrentBlock.AddStatement(new LocalAssignment {
                Names = [SymbolExpression.FromString("self")],
                Expressions = [FunctionCall.Basic("setmetatable", TableConstructor.Empty(), SymbolExpression.FromString(classSymbol.Name))],
                Types = [BasicTypeInfo.FromString($"_Instance{classSymbol.Name}")],
            });
            
            CurrentBlock.AddStatement(new FunctionCallStatement {
                Prefix = NamePrefix.FromString("self"),
                Suffixes = [new MethodCall { Name = "constructor", Args = FunctionArgs.Empty() }],
            });

            CurrentBlock.AddStatement(new Return { Returns = [SymbolExpression.FromString("self")] });
        }

        return block;
    }

    private IEnumerable<Statement> CreateConstructors(INamedTypeSymbol classSymbol) {
        foreach (var ctor in classSymbol.InstanceConstructors) {
            Log.Debug("Found constructor for {ClassName}", classSymbol.Name);

            if (ctor.IsImplicitlyDeclared) {
                // always parameterless
                yield return CreateParameterlessConstructor(classSymbol);
            } else {
                Log.Debug("Constructor is user-defined, injecting field initializers");

                yield return CreateUserDefinedConstructor(classSymbol, ctor);
            }
        }
    }

    private FunctionDeclaration CreateUserDefinedConstructor(INamedTypeSymbol classSymbol, IMethodSymbol constructor) {
        Log.Debug("Creating user-defined constructor for {ClassName}", classSymbol.Name);

        var functionBlock = Block.Empty();
        var functionNode = new FunctionDeclaration {
            Name = FunctionName.FromString($"{classSymbol.Name}:constructor"), Body = new FunctionBody {
                Body = functionBlock,
                Parameters = [],
                TypeSpecifiers = [],
                ReturnType = BasicTypeInfo.Void(),
            },
        };

        using (WithBlock(functionBlock, $"FunctionCtor_{classSymbol.Name}")) {
            var ctorParams = constructor.Parameters;

            var pars = ctorParams.Select(p => p.Name).Select(Parameter (pn) => NameParameter.FromString(pn));
            functionNode.Body.Parameters = pars.ToList();

            var typeSpecs = ctorParams.Select(p => p.Type).Select(TypeInfo (ts) => SyntaxUtilities.BasicFromSymbol(ts));
            functionNode.Body.TypeSpecifiers = typeSpecs.ToList();

            Log.Debug("Created constructor signature");

            var fields = classSymbol.GetMembers().OfType<IFieldSymbol>();
            var assignments = CreateFieldAssignmentsFromFields(fields);

            foreach (var assignment in assignments) CurrentBlock.AddStatement(assignment);
        }

        return functionNode;
    }

    private FunctionDeclaration CreateParameterlessConstructor(INamedTypeSymbol classSymbol) {
        Log.Debug("Creating parameterless constructor for {ClassName}", classSymbol.Name);

        var functionBlock = Block.Empty();
        using (WithBlock(functionBlock, $"FunctionParameterlessCtor_{classSymbol.Name}")) {
            var fields = classSymbol.GetMembers().OfType<IFieldSymbol>();
            var assignments = CreateFieldAssignmentsFromFields(fields);

            foreach (var assignment in assignments) CurrentBlock.AddStatement(assignment);
        }

        var functionNode = new FunctionDeclaration {
            Name = FunctionName.FromString($"{classSymbol.Name}:constructor"), Body = new FunctionBody {
                Body = functionBlock,
                Parameters = [],
                TypeSpecifiers = [],
                ReturnType = BasicTypeInfo.Void(),
            },
        };

        return functionNode;
    }

    private IEnumerable<Assignment> CreateFieldAssignmentsFromFields(IEnumerable<IFieldSymbol> fields) {
        foreach (var field in fields) {
            if (field.IsStatic) {
                Log.Warning("TODO: Implement static fields");
            }

            foreach (var declRef in field.DeclaringSyntaxReferences) {
                if (declRef.GetSyntax() is not VariableDeclaratorSyntax v) continue;

                var init = v.Initializer;
                if (init is null) continue;

                var rhs = LowerExpr(init.Value);

                yield return new Assignment {
                    Vars = [VarName.FromString($"self.{field.Name}")],
                    Expressions = [rhs],
                };
            }
        }
    }

    private (TypeDeclaration instanceDecl, TypeDeclaration typeDecl, TypeField ctorField, TypeField newField) BuildClassTypeDeclarations(ClassDeclarationSyntax node) {
        Log.Debug("Building type declaration for {ClassName}", node.Identifier.ValueText);

        var className = node.Identifier.ValueText;

        var instanceDecl = TypeDeclaration.EmptyTable($"_Instance{className}");
        using (UseType(instanceDecl)) {
            VisitMembers(node.Members, this);
        }

        var ctorField = BuildConstructorField(node, instanceDecl.Name);

        using (UseType(instanceDecl)) {
            var decl = instanceDecl.DeclareAs as TableTypeInfo;
            decl?.Fields.Add(ctorField);
        }

        var typeDecl = TypeDeclaration.EmptyTable($"_Type{className}");
        using (UseType(typeDecl)) {
            // todo: visit statics
            Log.Warning("TODO: Visit static");
        }

        return (instanceDecl, typeDecl, ctorField, ctorField);
    }

    private TypeField BuildConstructorField(ClassDeclarationSyntax node, string instanceTypeName) {
        Log.Debug("Attempting to build constructor callback for {ClassName}", node.Identifier.ValueText);

        var classSymbol = Semantics.GetDeclaredSymbol(node);
        if (classSymbol is null) return DefaultCtor();

        var ctorSymbol = classSymbol.Constructors.FirstOrDefault(c => !c.IsStatic);
        if (ctorSymbol is null) return DefaultCtor();

        var parameters = ctorSymbol.Parameters.Select(p =>
            TypeArgument.From(p.Name, SyntaxUtilities.BasicFromSymbol(p.Type))
        ).ToList();

        var ctorType = new CallbackTypeInfo {
            Arguments = parameters.Prepend(TypeArgument.From("self", BasicTypeInfo.FromString(instanceTypeName))).ToList(),
            ReturnType = BasicTypeInfo.Void(),
        };

        return TypeField.FromNameAndType("constructor", ctorType);

        TypeField DefaultCtor() {
            var cb = new CallbackTypeInfo {
                Arguments = [],
                ReturnType = BasicTypeInfo.Void(),
            };

            return TypeField.FromNameAndType("constructor", cb);
        }
    }

    private IEnumerable<TypeField> GenerateTypeFieldsFromField(FieldDeclarationSyntax fieldSyntax) {
        Log.Debug("Generating {FieldCount} type field(s)", fieldSyntax.Declaration.Variables.Count);

        var decl = fieldSyntax.Declaration;
        var fieldType = InferNonnull(decl.Type);
        var primitiveType = BasicTypeInfo.FromString(SyntaxUtilities.MapPrimitive(fieldType));

        var isReadonly = fieldSyntax.Modifiers.Any(SyntaxKind.ReadOnlyKeyword);

        foreach (var v in decl.Variables) {
            yield return new TypeField {
                Key = NameTypeFieldKey.FromString(v.Identifier.ValueText),
                Access = isReadonly ? AccessModifier.Read : null,
                Value = primitiveType,
            };
        }
    }

    private ITypeSymbol InferNonnull(TypeSyntax syntax) {
        var fieldType = Semantics.GetTypeInfo(syntax).Type!;
        if (fieldType is IErrorTypeSymbol or null) throw new Exception("Error occured while attempting to infer type.");

        Log.Verbose("Inferred type {Type} from {Name}", syntax.ToString(), fieldType.Name);

        return fieldType;
    }

    private bool WithTableFields(Action<TableTypeInfo> action) {
        if (ReferenceEquals(CurrentType, NoTypeSentinel)) return false;
        if (CurrentType.DeclareAs is not TableTypeInfo table) return false;

        action(table);

        return true;
    }

    private Scope WithBlock(Block block, string? name = null) {
        var scope = new Scope(_blockStack, block, name);
        Log.Verbose("Starting to populate {Name}", scope.GetFriendlyName());

        return scope;
    }

    private SetterGuard<TypeDeclaration> UseType(TypeDeclaration next) => new(t => CurrentType = t, CurrentType, next);

    private static void VisitMembers(SyntaxList<MemberDeclarationSyntax> members, CSharpSyntaxWalker walker) {
        foreach (var member in members) {
            walker.Visit(member);
        }
    }

    private static Expression LowerExpr(ExpressionSyntax syntax) => syntax switch {
        LiteralExpressionSyntax literal when literal.IsKind(SyntaxKind.StringLiteralExpression) => StringExpression.FromString(literal.Token.ValueText),
        LiteralExpressionSyntax lit when lit.IsKind(SyntaxKind.NumericLiteralExpression) => new NumberExpression { Value = Convert.ToDouble(lit.Token.Value) },
        LiteralExpressionSyntax lit when lit.IsKind(SyntaxKind.TrueLiteralExpression) => new BooleanExpression { Value = true },
        LiteralExpressionSyntax lit when lit.IsKind(SyntaxKind.FalseLiteralExpression) => new BooleanExpression { Value = false },
        _ => throw new ArgumentOutOfRangeException(nameof(syntax)),
    };
}