using RobloxCS.HIR;
using RobloxCS.Transpiler.Passes.Visitors;

namespace RobloxCS.Transpiler.Passes;
 
public sealed class HirPrinterPass : IPass {
    public string Name => "HIR Printer";
    public List<string> Diagnostics { get; } = [];
 
    public HirModule Run(HirModule module, TranspilationContext ctx) {
        new HirPrinter().VisitModule(module);
 
        return module;
    }
}