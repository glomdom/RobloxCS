#nullable enable
namespace RobloxCS.Types;
[RobloxNative("NetworkReplicator", RobloxNativeType.Instance)]
public partial class NetworkReplicator : Instance  {
    public Instance GetPlayer() => throw new InvalidOperationException("Cannot call reserved method for RobloxCS transpiler.");
}
