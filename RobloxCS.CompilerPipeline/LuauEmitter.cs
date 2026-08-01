using RobloxCS.Compiler;
using RobloxCS.Renderer;
using RobloxCS.Transpiler;

namespace RobloxCS.CompilerPipeline;

public static class LuauEmitter {
    public static string Emit(CSharpCompiler compiler, ScriptType scriptType) {
        var transpiler = new CSharpTranspiler(new TranspilerOptions(scriptType), compiler);

        return new RendererWalker().Render(transpiler.Transpile());
    }
}