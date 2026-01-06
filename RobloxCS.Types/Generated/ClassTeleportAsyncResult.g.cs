#nullable enable
namespace RobloxCS.Types;
[RobloxNative("TeleportAsyncResult", RobloxNativeType.Instance)]
public partial class TeleportAsyncResult : Instance  {
    public string PrivateServerId { get; set; } = default!;
    public string ReservedServerAccessCode { get; set; } = default!;
}
