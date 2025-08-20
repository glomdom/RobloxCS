using RobloxCS.AST;
using RobloxCS.AST.Expressions;
using RobloxCS.AST.Statements;

namespace RobloxCS.Renderer.Renderers;

public class AssignmentRenderer : IRenderer<Assignment> {
    public void Render(RenderState state, Assignment node) {
        for (var i = 0; i < node.Vars.Count; i++) {
            var renderer = state.GetRenderer<Var>();
            renderer.Render(state, node.Vars[i]);

            if (i < node.Vars.Count - 1) {
                state.Builder.Append(", ");
            }
        }

        state.Builder.Append(" = ");
        
        for (var i = 0; i < node.Expressions.Count; i++) {
            var renderer = state.GetRenderer<Expression>();
            renderer.Render(state, node.Expressions[i]);

            if (i < node.Expressions.Count - 1) {
                state.Builder.Append(", ");
            }
        }

        state.Builder.AppendLine();
    }
}