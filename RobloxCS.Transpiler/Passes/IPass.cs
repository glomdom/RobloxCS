namespace RobloxCS.Transpiler.Passes;

public interface IPass {
    string Name { get; }
    
    void Run(TranspilationContext ctx);
    void PostRun(TranspilationContext ctx) { }
}