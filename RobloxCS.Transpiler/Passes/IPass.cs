using RobloxCS.HIR;

namespace RobloxCS.Transpiler.Passes;

public interface IPass {
    string Name { get; }
    List<string> Diagnostics { get; }
    
    HirModule Run(HirModule module, TranspilationContext ctx);
}