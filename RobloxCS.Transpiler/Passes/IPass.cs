namespace RobloxCS.Transpiler.Passes;

public interface IPass {
    string Name { get; }
    List<string> Diagnostics { get; }
    
    void Run(TranspilationContext ctx);
    void PostRun(TranspilationContext ctx) { }
}