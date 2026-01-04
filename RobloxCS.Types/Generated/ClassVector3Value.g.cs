#nullable enable
namespace RobloxCS.Types;
[RobloxNative("Vector3Value", RobloxNativeType.Instance)]
public partial class Vector3Value : ValueBase  {
    public Vector3 Value { get; } = default!;
    new public RBXScriptSignal<Vector3> Changed { get; private set; } = null!;
}
