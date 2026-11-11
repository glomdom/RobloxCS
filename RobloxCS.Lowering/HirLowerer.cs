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
    private const string EntryPointLocal = "__entrypointCls";

    public ScriptType ScriptType { get; }
    public DiagnosticBag Diagnostics { get; }
    public ModuleGraph Modules { get; }

    private readonly LoweringContext _names = new();
    private readonly HashSet<INamedTypeSymbol> _moduleTypes = new(SymbolEqualityComparer.Default);
    private readonly HashSet<INamedTypeSymbol> _imports = new(SymbolEqualityComparer.Default);
    private readonly HashSet<string> _importNames = [];

    public HirLowerer(ScriptType scriptType, ModuleGraph modules, DiagnosticBag diagnostics) {
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

        var statements = new List<Statement>();
        var lowered = new HashSet<INamedTypeSymbol>(SymbolEqualityComparer.Default);
        foreach (var type in module.Types.OrderBy(type => type.Symbol.TypeKind == TypeKind.Enum ? 0 : 1)) {
            LowerWithBase(type);
        }

        void LowerWithBase(RobloxCS.HIR.Declarations.HirType type) {
            if (!lowered.Add(type.Symbol)) {
                return;
            }

            var parent = module.Types.FirstOrDefault(candidate => SymbolEqualityComparer.Default.Equals(candidate.Symbol, type.Symbol.BaseType));
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

        statements.InsertRange(0, LowerImports());

        return new Chunk {
            Mode = TypeCheckMode.Strict,
            Block = new Block {
                Statements = statements,
            },
        };
    }

    private static string TypeName(INamedTypeSymbol type) => type.Name;

    private void Unsupported(string message, Location location) {
        Log.Debug("Reporting unsupported construct: {Message}", message);

        Diagnostics.Error(DiagnosticId.UnsupportedSyntax, message, location);
    }
}
