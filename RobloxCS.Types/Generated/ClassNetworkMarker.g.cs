#nullable enable
namespace RobloxCS.Types;
[RobloxNative("NetworkMarker", RobloxNativeType.Instance)]
public partial class NetworkMarker : Instance  {
    public RBXScriptSignal Received { get; private set; } = null!;
}
