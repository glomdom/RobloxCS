#nullable enable
namespace RobloxCS.Types;
[RobloxNative("ArcHandles", RobloxNativeType.Instance)]
public partial class ArcHandles : HandlesBase  {
    public Axes Axes { get; } = default!;
    public RBXScriptSignal<Enums.Axis> MouseButton1Down { get; private set; } = null!;
    public RBXScriptSignal<Enums.Axis> MouseButton1Up { get; private set; } = null!;
    public RBXScriptSignal<Enums.Axis, float, float> MouseDrag { get; private set; } = null!;
    public RBXScriptSignal<Enums.Axis> MouseEnter { get; private set; } = null!;
    public RBXScriptSignal<Enums.Axis> MouseLeave { get; private set; } = null!;
}
