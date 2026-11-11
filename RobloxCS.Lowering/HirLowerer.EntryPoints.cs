using RobloxCS.AST.Statements;
using RobloxCS.HIR;
using Serilog;
using static RobloxCS.Lowering.Luau;

namespace RobloxCS.Lowering;

public sealed partial class HirLowerer {
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
        var className = TypeName(owner.Symbol);

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
}
