using Microsoft.CodeAnalysis;
using RobloxCS.AST;
using RobloxCS.AST.Expressions;
using RobloxCS.AST.Statements;
using RobloxCS.Common;
using RobloxCS.Common.Diagnostics;
using RobloxCS.HIR;
using RobloxCS.Modules;
using Serilog;
using static RobloxCS.Lowering.Luau;

namespace RobloxCS.Lowering;

public sealed partial class HirLowerer {
    private readonly string _runtimeFolder;

    private const string EntryPointLocal = "__entrypointCls";

    public ScriptType ScriptType { get; }
    public DiagnosticBag Diagnostics { get; }
    public ModuleGraph Modules { get; }

    private readonly LoweringContext _names = new();
    private readonly HashSet<INamedTypeSymbol> _moduleTypes = new(SymbolEqualityComparer.Default);
    private readonly HashSet<INamedTypeSymbol> _imports = new(SymbolEqualityComparer.Default);
    private readonly HashSet<string> _importNames = [];

    /// <param name="runtimeFolder">Require path of the folder holding the runtime modules, without a trailing slash.</param>
    public HirLowerer(ScriptType scriptType, ModuleGraph modules, DiagnosticBag diagnostics, string runtimeFolder = RuntimeLibrary.RequireFolder) {
        _runtimeFolder = runtimeFolder;
        ScriptType = scriptType;
        Diagnostics = diagnostics;
        Modules = modules;
    }

    public Chunk Lower(HirModule module) {
        using var _ = LoggerSetup.PushPass("Lowering");

        Log.Information("Lowering {SourcePath} as a {ScriptType} script ({TypeCount} types)", module.SourcePath, ScriptType, module.Types.Length);

        foreach (var type in module.Types) {
            _moduleTypes.Add(type.Symbol);
        }

        var identifiers = module.Types.SelectMany(type => type.Symbol.DeclaringSyntaxReferences)
            .Select(reference => reference.SyntaxTree).Distinct()
            .SelectMany(tree => tree.GetRoot().DescendantTokens()).Select(token => token.ValueText).ToHashSet();

        _collectionsName = ReserveRuntimeName("__collections", identifiers);
        _typesName = ReserveRuntimeName("__types", identifiers);

        var statements = module.Types.Where(type => type.Symbol.TypeKind == TypeKind.Class)
            .Select(type => (Statement)Local(TypeName(type.Symbol), Table())).ToList();

        var lowered = new HashSet<INamedTypeSymbol>(SymbolEqualityComparer.Default);
        foreach (var type in module.Types.OrderBy(type => type.Symbol.TypeKind is TypeKind.Enum or TypeKind.Interface ? 0 : 1)) {
            LowerWithBase(type);
        }

        void LowerWithBase(RobloxCS.HIR.Declarations.HirType type) {
            if (!lowered.Add(type.Symbol)) {
                return;
            }

            var parent = module.Types.FirstOrDefault(candidate => SymbolEqualityComparer.Default.Equals(candidate.Symbol, type.Symbol.BaseType?.OriginalDefinition));
            if (parent is not null) {
                LowerWithBase(parent);
            }

            statements.AddRange(LowerTypeDeclaration(type));
        }

        statements.AddRange(LowerEntryPoint(module));

        if (ScriptType == ScriptType.Module) {
            statements.Add(Return(new TableConstructorExpression {
                Fields = [
                    ..module.Types
                        .Where(type => type.Symbol.TypeKind is TypeKind.Class or TypeKind.Enum)
                        .Select(TableField (type) => new NameKey { Key = TypeName(type.Symbol), Value = Name(TypeName(type.Symbol)) }),
                ],
            }));
        }

        Log.Information("Lowered {SourcePath} into {StatementCount} top-level statements", module.SourcePath, statements.Count);

        // descriptors only close over class tables, so they can follow the class locals
        statements.InsertRange(module.Types.Count(type => type.Symbol.TypeKind == TypeKind.Class), _descriptorStatements);

        statements.InsertRange(0, LowerImports());

        if (_usesTypes) {
            statements.Insert(0, Local(_typesName, Call("require", Str($"{_runtimeFolder}/{RuntimeLibrary.TypesModule}"))));
        }

        if (_usesCollections) {
            statements.Insert(0, Local(_collectionsName, Call("require", Str($"{_runtimeFolder}/{RuntimeLibrary.CollectionsModule}"))));
        }

        return new Chunk {
            Mode = TypeCheckMode.Strict,
            Block = new Block {
                Statements = statements,
            },
        };
    }

    private string ReserveRuntimeName(string name, HashSet<string> identifiers) {
        while (identifiers.Contains(name)) {
            name += "_";
        }

        _names.ReserveGlobal(name);
        _importNames.Add(name);

        return name;
    }

    // Generic arity is part of a class's identity: Box, Box<T> and Box<T1, T2> are three classes and need three Luau names.
    private static string TypeName(INamedTypeSymbol type) => type.TypeKind == TypeKind.Interface
        ? type.InterfaceTypeName()
        : type.Arity > 0 ? $"{type.Name}_{type.Arity}" : type.Name;

    private void Unsupported(string message, Location location) {
        Log.Debug("Reporting unsupported construct: {Message}", message);

        Diagnostics.Error(DiagnosticId.UnsupportedSyntax, message, location);
    }
}