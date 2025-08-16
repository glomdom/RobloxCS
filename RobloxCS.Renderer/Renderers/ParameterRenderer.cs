using RobloxCS.AST;

namespace RobloxCS.Renderer.Renderers;

public class ParameterRenderer : IRenderer<Parameter> {
    public void Render(RenderState state, Parameter node) {
        switch (node) {
            case EllipsisParameter: {
                state.Builder.Append("...");

                break;
            }

            case NameParameter name: {
                state.Builder.Append(name.Name);

                break;
            }

            default: throw new Exception($"Unhandled parameter type: {node.GetType().Name}");
        }
    }
}