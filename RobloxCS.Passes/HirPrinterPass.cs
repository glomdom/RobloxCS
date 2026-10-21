using RobloxCS.HIR;
using RobloxCS.Passes.Visitors;

namespace RobloxCS.Passes;

/// <summary>
/// Debug pass that dumps the HIR tree to the console. Does not modify the module.
/// </summary>
public sealed class HirPrinterPass : IPass {
    public string Name => "HIR Printer";

    public HirModule Run(HirModule module, PassContext ctx) {
        new HirPrinter().VisitModule(module);

        return module;
    }
}
