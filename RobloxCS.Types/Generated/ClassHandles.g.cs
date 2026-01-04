#nullable enable
namespace RobloxCS.Types;
[RobloxNative("Handles", RobloxNativeType.Instance)]
public partial class Handles : HandlesBase  {
    public Faces Faces { get; } = default!;
    public Enums.HandlesStyle Style { get; } = default!;
    public RBXScriptSignal<Enums.NormalId> MouseButton1Down { get; private set; } = null!;
    public RBXScriptSignal<Enums.NormalId> MouseButton1Up { get; private set; } = null!;
    public RBXScriptSignal<Enums.NormalId, float> MouseDrag { get; private set; } = null!;
    public RBXScriptSignal<Enums.NormalId> MouseEnter { get; private set; } = null!;
    public RBXScriptSignal<Enums.NormalId> MouseLeave { get; private set; } = null!;
}
