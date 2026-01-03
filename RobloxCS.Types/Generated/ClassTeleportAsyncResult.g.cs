#nullable enable
namespace RobloxCS.Types;
[RobloxNative("TeleportAsyncResult", RobloxNativeType.Instance)]
public partial class TeleportAsyncResult : Instance  {
    public string PrivateServerId { get; } = default!;
    public string ReservedServerAccessCode { get; } = default!;
}
