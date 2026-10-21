using RobloxCS.HIR;

namespace RobloxCS.Passes;

public interface IPass {
    string Name { get; }

    HirModule Run(HirModule module, PassContext ctx);
}
