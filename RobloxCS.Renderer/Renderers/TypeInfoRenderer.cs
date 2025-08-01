using RobloxCS.AST.Types;
using RobloxCS.Common;

namespace RobloxCS.Renderer.Renderers;

public class TypeInfoRenderer : IRenderer<TypeInfo> {
    public void Render(RenderState state, TypeInfo node) {
        if (node is TableTypeInfo table) {
            state.Builder.AppendLine("{");
            state.PushIndent();

            foreach (var field in table.Fields) {
                var hasModifier = field.Access is not null;
                
                if (hasModifier) {
                    state.AppendIndented(field.Access!.Value.ToFriendlyString());
                } else {
                    state.AppendIndent();
                }

                state.GetRenderer<TypeFieldKey>().Render(state, field.Key);
                state.Builder.Append(": ");
                Render(state, field.Value);
                state.Builder.Append(',');
                state.Builder.AppendLine();
            }
            
            state.PopIndent();
            state.Builder.Append('}');
        } else if (node is BasicTypeInfo basic) {
            state.Builder.Append(basic.Name);
        }
    }
}