using System.Collections;
using System.Collections.Immutable;
using System.Text;
using RobloxCS.AST;
using RobloxCS.AST.Statements;
using RobloxCS.AST.Types;
using RobloxCS.Renderer.Renderers;

namespace RobloxCS.Renderer;

public class RenderState {
    public string Indent => new(' ', _indentLevel * IndentCharacter.Length);
    public StringBuilder Builder = new();

    private ImmutableDictionary<Type, IRenderer> Renderers { get; } = ImmutableDictionary.CreateRange(
        [
            AdapterForNode<LocalAssignment, LocalAssignmentRenderer>(),
            AdapterForNode<Block, BlockRenderer>(),
            AdapterForNode<Return, ReturnRenderer>(),
            AdapterForNode<DoStatement, DoRenderer>(),
            AdapterForNode<Assignment, AssignmentRenderer>(),
            AdapterForNode<Var, VarRenderer>(),
            AdapterForNode<Expression, ExpressionRenderer>(),
            AdapterForNode<TypeDeclaration, TypeDeclarationRenderer>(),
            AdapterForNode<TypeInfo, TypeInfoRenderer>(),
            AdapterForNode<TypeFieldKey, TypeFieldKeyRenderer>(),
            AdapterForNode<TypeArgument, TypeArgumentRenderer>(),
        ]
    );

    private const string IndentCharacter = "  ";
    private int _indentLevel;

    public void PushIndent() => _indentLevel++;
    public void PopIndent() => _indentLevel = Math.Max(0, _indentLevel - 1);
    public void AppendIndent() => Builder.Append(Indent);
    public void AppendIndented(string text) => Builder.Append($"{Indent}{text}");
    public void AppendIndentedLine(string text) => Builder.AppendLine($"{Indent}{text}");

    public void RenderPunctuated<T>(IList<T> list, RenderState state) where T : AstNode {
        for (var i = 0; i < list.Count; i++) {
            var renderer = GetRenderer(typeof(T));
            renderer.Render(state, list[i]);

            if (i != list.Count - 1) {
                state.Builder.Append(", ");
            }
        }
    }

    /// <summary>
    /// Gets a renderer from the provided <c>type</c>.
    /// </summary>
    /// <returns>A renderer, if it was found.</returns>
    /// <exception cref="NotImplementedException">Thrown when a renderer was not found.</exception>
    public IRenderer GetRenderer(Type type) {
        if (!Renderers.TryGetValue(type, out var renderer)) {
            throw new RendererNotFoundException("Renderer is not registered/implemented.", type.Name);
        }

        return renderer;
    }

    /// <summary>
    /// Gets a renderer from the provided <c>type</c>.
    /// </summary>
    /// <returns>A typed renderer, if it was found.</returns>
    /// <exception cref="NotImplementedException">Thrown when a renderer was not found.</exception>
    /// <exception cref="InvalidCastException">Thrown if a renderer was found, but does not match type constraint.</exception>
    public IRenderer<T> GetRenderer<T>() where T : AstNode {
        var renderer = GetRenderer(typeof(T));
        if (renderer is IRenderer<T> tRenderer) {
            return tRenderer;
        }

        // this shouldnt happen but better safe than sorry. TODO: probably add a catch for this somewhere
        throw new InvalidCastException($"Renderer for type {typeof(T).Name} is not of expected type IRenderer<{typeof(T).Name}>.");
    }

    private static KeyValuePair<Type, IRenderer> AdapterForNode<TNode, TRenderer>()
        where TNode : AstNode where TRenderer : IRenderer<TNode>, new() {
        return new KeyValuePair<Type, IRenderer>(typeof(TNode), new RendererAdapter<TNode>(new TRenderer()));
    }
}