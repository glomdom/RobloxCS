using RobloxCS.AST;
using RobloxCS.AST.Expressions;

namespace RobloxCS.Renderer.Renderers;

public class ExpressionRenderer : IRenderer<Expression> {
    public void Render(RenderState state, Expression node) {
        if (node is BooleanExpression boolean) {
            RenderBoolean(state, boolean);
        } else if (node is SymbolExpression sym) {
            RenderSymbol(state, sym);
        } else if (node is FunctionCall func) {
            RenderFunctionCall(state, func);
        } else if (node is TableConstructor tctor) {
            RenderTableConstructor(state, tctor);
        } else {
            throw new Exception($"Unhandled expression type: {node.GetType().Name}");
        }
    }

    private static void RenderBoolean(RenderState state, BooleanExpression expr) {
        state.Builder.Append(expr.Value ? "true" : "false");
    }

    private static void RenderSymbol(RenderState state, SymbolExpression sym) {
        state.Builder.Append(sym.Value);
    }

    private static void RenderFunctionCall(RenderState state, FunctionCall call) {
        var prefixRenderer = state.GetRenderer<Prefix>();
        prefixRenderer.Render(state, call.Prefix);

        var suffixRenderer = state.GetRenderer<Suffix>();
        foreach (var suffix in call.Suffixes) {
            suffixRenderer.Render(state, suffix);
        }
    }

    // TODO: implement field rendering
    private static void RenderTableConstructor(RenderState state, TableConstructor ctor) {
        state.Builder.Append('{');
        
        state.Builder.Append('}');
    }
}