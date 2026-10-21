using RobloxCS.AST;
using RobloxCS.Common.Diagnostics;
using RobloxCS.HIR;

namespace RobloxCS.Lowering;

/// <summary>
/// Lowers a <see cref="HirModule"/> into a Luau <see cref="Chunk"/>.
/// </summary>
public sealed class HirLowerer {
    public ScriptType ScriptType { get; }
    public DiagnosticBag Diagnostics { get; }

    public HirLowerer(ScriptType scriptType, DiagnosticBag diagnostics) {
        ScriptType = scriptType;
        Diagnostics = diagnostics;
    }

    public Chunk Lower(HirModule module) {
        // Not implemented yet: emits an empty chunk, same as before the HIR rewrite reached this stage.
        // TODO: prologue should require the runtime List:
        //   local List = require(game:GetService("ReplicatedStorage"):WaitForChild("robloxcs"):WaitForChild("RuntimeLib"):WaitForChild("List"))
        return new Chunk {
            Block = new Block {
                Statements = [],
            },
        };
    }
}
