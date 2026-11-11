using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.CodeAnalysis.Operations;
using RobloxCS.HIR.Declarations;
using RobloxCS.HIR.Expressions;
using RobloxCS.HIR.Statements;
using Serilog;

namespace RobloxCS.HIR.Builder;

public sealed partial class HirBuilder {
    public HirMethod BuildMethod(IMethodSymbol method) {
        Log.Verbose("Adding method {MethodName} of kind {MethodKind}", method.Name, method.MethodKind);

        var isCtor = method is { MethodKind: MethodKind.Constructor };
        var isImplicitCtor = method is { IsImplicitlyDeclared: true } && isCtor;
        var isEntryPoint = IsEntryPoint(method);

        var parameters = method.Parameters.Select(BuildParameter).ToList();
        var block = isImplicitCtor || method.IsPrimaryConstructor() || method.IsAbstract ? new HirBlock { Location = Location.None, Statements = [], Locals = [] } : BuildMethodBlock(method);

        return new HirMethod {
            Location = method.Locations.ResolveSingle(),
            Symbol = method,
            Initializer = isCtor ? BuildConstructorInitializer(method) : null,
            Parameters = [.. parameters],
            TypeParameters = [], // todo
            Block = block,
            IsStatic = method.IsStatic,
            IsConstructor = isCtor,
            IsEntryPoint = isEntryPoint,
            IsImplicit = method is { IsImplicitlyDeclared: true },
        };
    }

    private HirCall? BuildConstructorInitializer(IMethodSymbol method) {
        var syntax = method.DeclaringSyntaxReferences.FirstOrDefault()?.GetSyntax();
        if (syntax is ClassDeclarationSyntax { BaseList: { } baseList } && method.IsPrimaryConstructor()) {
            var primaryBase = baseList.Types.OfType<PrimaryConstructorBaseTypeSyntax>().FirstOrDefault();
            if (primaryBase is not null) {
                var baseInvocation = Semantics.CheckedGetOperation<IInvocationOperation>(primaryBase);

                if (baseInvocation.TargetMethod.ContainingType.SpecialType != SpecialType.System_Object) {
                    return (HirCall)BuildExpression(baseInvocation);
                }
            }
        }

        if (syntax is ConstructorDeclarationSyntax constructor &&
            Semantics.GetOperation(constructor) is IConstructorBodyOperation { Initializer: IExpressionStatementOperation { Operation: IInvocationOperation invocation } }) {
            if (invocation.TargetMethod.ContainingType.SpecialType != SpecialType.System_Object) {
                return (HirCall)BuildExpression(invocation);
            }
        }

        if (method.ContainingType.BaseType is not { SpecialType: not SpecialType.System_Object } baseType) {
            return null;
        }

        var target = baseType.InstanceConstructors.FirstOrDefault(ctor => ctor.Parameters.Length == 0)
            ?? baseType.InstanceConstructors.First(ctor => ctor.Parameters.All(parameter => parameter.HasExplicitDefaultValue));

        return new HirCall {
            Location = method.Locations.ResolveSingle(),
            Type = method.ReturnType,
            Method = target,
            Receiver = null,
            Arguments = [.. target.Parameters.Select(parameter => new HirArgument {
                Location = method.Locations.ResolveSingle(),
                Symbol = parameter,
                Value = new HirLiteral {
                    Location = method.Locations.ResolveSingle(),
                    Type = parameter.Type,
                    Value = parameter.ExplicitDefaultValue,
                },
            })],
            IsExtension = false,
        };
    }

    private bool IsEntryPoint(IMethodSymbol method) {
        var entryPointAttr = Compiler.Types.EntryPointAttributeSymbol;

        return method.GetAttributes().Any(attr => SymbolEqualityComparer.Default.Equals(attr.AttributeClass, entryPointAttr));
    }

    private HirBlock BuildMethodBlock(IMethodSymbol method) {
        var syntax = method.DeclaringSyntaxReferences.FirstOrDefault()?.GetSyntax();

        var body = syntax switch {
            BaseMethodDeclarationSyntax or AccessorDeclarationSyntax => Semantics.GetOperation(syntax) is IMethodBodyBaseOperation operation
                ? operation.BlockBody ?? operation.ExpressionBody
                : null,
            ArrowExpressionClauseSyntax arrow => Semantics.GetOperation(arrow) as IBlockOperation,
            _ => null,
        };

        if (body is null) {
            var error = Unsupported("this kind of method body is not supported", method.Locations.ResolveSingle());

            return new HirBlock { Location = Location.None, Statements = [error], Locals = [] };
        }

        return new HirBlock {
            Location = Location.None,
            Statements = [.. body.Operations.Select(BuildStatement)],
            Locals = [.. body.Locals],
        };
    }
}
