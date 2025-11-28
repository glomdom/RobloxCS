namespace RobloxCS.Transpiler.Passes;

public interface IPass {
    void Run(TranspilationContext ctx);
}