using RobloxCS.AST;
using RobloxCS.AST.Expressions;

namespace RobloxCS.Renderer.Renderers;

public class ExpressionRenderer : IRenderer<Expression> {
    public void Render(RenderState state, Expression node) {
        if (node is BooleanExpression boolean) {
            RenderBoolean(state, boolean);
        } else if (node is SymbolExpression sym) {
            RenderSymbol(state, sym);
        } else if (node is StringExpression str) {
            RenderString(state, str);
        } else if (node is FunctionCall func) {
            RenderFunctionCall(state, func);
        } else if (node is TableConstructor tctor) {
            RenderTableConstructor(state, tctor);
        } else if (node is AnonymousFunction anonFunc) {
            RenderAnonymousFunction(state, anonFunc);
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

    private void RenderString(RenderState state, StringExpression str) {
        state.Builder.Append($"\"{str.Value}\"");
    }

    private static void RenderFunctionCall(RenderState state, FunctionCall call) {
        var prefixRenderer = state.GetRenderer<Prefix>();
        prefixRenderer.Render(state, call.Prefix);

        var suffixRenderer = state.GetRenderer<Suffix>();
        foreach (var suffix in call.Suffixes) {
            suffixRenderer.Render(state, suffix);
        }
    }

    private static void RenderTableConstructor(RenderState state, TableConstructor ctor) {
        if (ctor.Fields.Count == 0) {
            state.Builder.Append("{}");

            return;
        }

        state.Builder.AppendLine("{");
        state.PushIndent();

        var expressionRenderer = state.GetRenderer<Expression>();

        foreach (var field in ctor.Fields) {
            switch (field) {
                case NoKey noKey: {
                    state.AppendIndent();
                    expressionRenderer.Render(state, noKey.Expression);

                    break;
                }

                case NameKey nameKey: {
                    state.AppendIndent();
                    state.Builder.Append(nameKey.Key);
                    state.Builder.Append(" = ");

                    expressionRenderer.Render(state, nameKey.Value);

                    break;
                }
            }
        }

        state.PopIndent();
        state.Builder.AppendLine();
        state.AppendIndented("}");
    }

    private void RenderAnonymousFunction(RenderState state, AnonymousFunction anonFunc) {
        var body = anonFunc.Body;

        state.Builder.Append("function(");
        state.RenderPunctuated(body.Parameters, state);
        state.Builder.Append(')');
        state.Builder.AppendLine();

        state.PushIndent();

        foreach (var stmt in body.Body.Statements) {
            var renderer = state.GetRenderer(stmt.GetType());
            renderer.Render(state, stmt);
        }

        state.PopIndent();
        state.Builder.AppendLine();
        state.AppendIndented("end");
    }
}