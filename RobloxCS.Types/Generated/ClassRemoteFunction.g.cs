#nullable enable
namespace RobloxCS.Types;
[RobloxNative("RemoteFunction", RobloxNativeType.Instance)]
public partial class RemoteFunction : Instance  {
    public LuaTuple InvokeClient() => throw new InvalidOperationException("Cannot call reserved method for RobloxCS transpiler.");
    public LuaTuple InvokeServer() => throw new InvalidOperationException("Cannot call reserved method for RobloxCS transpiler.");
}
