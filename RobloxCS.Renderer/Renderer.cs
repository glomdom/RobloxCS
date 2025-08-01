using System.Collections.Immutable;
using RobloxCS.AST;

#if !DEBUG
using System.Runtime.ExceptionServices;
#endif

namespace RobloxCS.Renderer;

public class Renderer {
    private ImmutableList<AstNode> Nodes { get; init; }

    public Renderer(ImmutableList<AstNode> nodes) {
        Nodes = nodes;
    }

    public Renderer(List<AstNode> nodes) {
        Nodes = [..nodes];
    }

    /// <summary>
    /// Returns a line formatted string representing the entire provided AST in source code.
    /// </summary>
    public string Render() {
        var state = new RenderState();

        foreach (var node in Nodes) {
            try {
                var renderer = state.GetRenderer(node.GetType());
                renderer.Render(state, node);
            } catch (RendererNotFoundException e) {
#if DEBUG
                Console.WriteLine($"Renderer {e.RendererName} was not found, ignoring as this is a debug build.");

                continue;
#else
                ExceptionDispatchInfo.Capture(e).Throw();
                throw;
#endif
            }
        }

        return state.Builder.ToString();
    }
}