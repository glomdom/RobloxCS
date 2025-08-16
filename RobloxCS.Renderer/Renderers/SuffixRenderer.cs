using RobloxCS.AST;
using RobloxCS.AST.Suffixes;

namespace RobloxCS.Renderer.Renderers;

public class SuffixRenderer : IRenderer<Suffix> {
    public void Render(RenderState state, Suffix node) {
        if (node is AnonymousCall anon) {
            state.Builder.Append('(');
            state.RenderPunctuated(anon.Arguments.Arguments, state);
            state.Builder.Append(')');
        }
    }
}