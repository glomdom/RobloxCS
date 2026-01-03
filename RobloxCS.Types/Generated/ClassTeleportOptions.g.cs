#nullable enable
namespace RobloxCS.Types;
[RobloxNative("TeleportOptions", RobloxNativeType.Instance)]
public partial class TeleportOptions : Instance  {
    public string ReservedServerAccessCode { get; } = default!;
    public string ServerInstanceId { get; } = default!;
    public bool ShouldReserveServer { get; } = default!;
    public object GetTeleportData() => throw new InvalidOperationException("Cannot call reserved method for RobloxCS transpiler.");
    public void SetTeleportData() => throw new InvalidOperationException("Cannot call reserved method for RobloxCS transpiler.");
}
