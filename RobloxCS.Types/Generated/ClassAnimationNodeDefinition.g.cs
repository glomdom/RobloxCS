#nullable enable
namespace RobloxCS.Types;
[RobloxNative("AnimationNodeDefinition", RobloxNativeType.Instance)]
public partial class AnimationNodeDefinition : Instance  {
    public Enums.AnimationNodeType NodeType { get; } = default!;
    public List<Instance> GetConnectedWires() => throw new InvalidOperationException("Cannot call reserved method for RobloxCS transpiler.");
    public List<object> GetInputPins() => throw new InvalidOperationException("Cannot call reserved method for RobloxCS transpiler.");
    public List<object> GetOutputPins() => throw new InvalidOperationException("Cannot call reserved method for RobloxCS transpiler.");
}
