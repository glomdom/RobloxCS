using Microsoft.CodeAnalysis;
using RobloxCS.AST;
using RobloxCS.AST.Statements;
using RobloxCS.Common;
using RobloxCS.Common.Diagnostics;
using RobloxCS.HIR;
using Serilog;
using static RobloxCS.Lowering.Luau;

namespace RobloxCS.Lowering;

public sealed partial class HirLowerer {
    private const string EntryPointLocal = "__entrypointCls";

    public ScriptType ScriptType { get; }
    public DiagnosticBag Diagnostics { get; }

    private readonly LoweringContext _names = new();
    private readonly HashSet<INamedTypeSymbol> _moduleTypes = new(SymbolEqualityComparer.Default);

    public HirLowerer(ScriptType scriptType, DiagnosticBag diagnostics) {
        ScriptType = scriptType;
        Diagnostics = diagnostics;
    }

    public Chunk Lower(HirModule module) {
        // TODO: prologue should require the runtime List:
        //   local List = require(game:GetService("ReplicatedStorage"):WaitForChild("robloxcs"):WaitForChild("RuntimeLib"):WaitForChild("List"))

        using var _ = LoggerSetup.PushPass("Lowering");

        Log.Information("Lowering {SourcePath} as a {ScriptType} script ({TypeCount} types)", module.SourcePath, ScriptType, module.Types.Length);

        foreach (var type in module.Types) {
            _moduleTypes.Add(type.Symbol);
        }

        var statements = new List<Statement>();
        foreach (var type in module.Types) {
            statements.AddRange(LowerTopLevelType(type));
        }

        statements.AddRange(LowerEntryPoint(module));

        Log.Information("Lowered {SourcePath} into {StatementCount} top-level statements", module.SourcePath, statements.Count);

        return new Chunk {
            Mode = TypeCheckMode.Strict,
            Block = new Block {
                Statements = statements,
            },
        };
    }

    private List<Statement> LowerEntryPoint(HirModule module) {
        var entryPoints = module.Types
            .SelectMany(type => type.Methods.Where(method => method.IsEntryPoint).Select(method => (Type: type, Method: method)))
            .ToList();

        if (entryPoints.Count == 0) {
            Log.Debug("No entry point found, skipping entry point call");

            return [];
        }

        if (entryPoints.Count > 1) {
            foreach (var (_, method) in entryPoints.Skip(1)) {
                Unsupported("Only one [EntryPoint] per file is supported.", method.Location);
            }

            return [];
        }

        var (owner, entryPoint) = entryPoints[0];
        var className = ClassName(owner.Symbol);

        if (entryPoint.Parameters.Length > 0) {
            Unsupported("Entry points can't take parameters.", entryPoint.Location);

            return [];
        }

        if (entryPoint.IsStatic) {
            Log.Debug("Calling static entry point {ClassName}.{MethodName}", className, entryPoint.Symbol.Name);

            return [Stmt(Call(Index(className, entryPoint.Symbol.Name)))];
        }

        var constructor = owner.Methods.FirstOrDefault(method => method.IsConstructor);
        if (constructor is { Parameters.Length: > 0 }) {
            Unsupported("An instance entry point's class needs a parameterless constructor.", entryPoint.Location);

            return [];
        }

        Log.Debug("Instantiating {ClassName} into {Local} to call entry point {MethodName}", className, EntryPointLocal, entryPoint.Symbol.Name);

        return [
            Local(EntryPointLocal, Call(Index(className, "new"))),
            Stmt(MethodCall(Name(EntryPointLocal), entryPoint.Symbol.Name)),
        ];
    }

    private bool IsModuleType(ITypeSymbol type) => type is INamedTypeSymbol named && _moduleTypes.Contains(named);
    private static string ClassName(INamedTypeSymbol type) => type.Name;

    private void Unsupported(string message, Location location) {
        Log.Debug("Reporting unsupported construct: {Message}", message);

        Diagnostics.Error(DiagnosticId.UnsupportedSyntax, message, location);
    }
}