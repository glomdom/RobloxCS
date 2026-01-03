#nullable enable
namespace RobloxCS.Types;
[RobloxNative("AdPortal", RobloxNativeType.Instance)]
public partial class AdPortal : Instance  {
    public string PortalInvalidReason { get; } = default!;
    public Enums.AdUnitStatus Status { get; } = default!;
}
