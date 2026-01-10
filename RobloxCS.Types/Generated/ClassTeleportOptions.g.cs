#nullable enable
namespace RobloxCS.Types;
[RobloxNative("TeleportOptions", RobloxNativeType.Instance)]
public partial class TeleportOptions : Instance  {
    public string ReservedServerAccessCode { get; set; } = default!;
    public string ServerInstanceId { get; set; } = default!;
    public bool ShouldReserveServer { get; set; } = default!;
    public object GetTeleportData() => ThrowHelper.ThrowTranspiledMethod();
    public void SetTeleportData() => ThrowHelper.ThrowTranspiledMethod();
}
