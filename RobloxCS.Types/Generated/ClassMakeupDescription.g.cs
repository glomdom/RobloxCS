#nullable enable
namespace RobloxCS.Types;
[RobloxNative("MakeupDescription", RobloxNativeType.Instance)]
public partial class MakeupDescription : Instance  {
    public int AssetId { get; set; } = default!;
    public Instance Instance { get; set; } = default!;
    public Enums.MakeupType MakeupType { get; set; } = default!;
    public int Order { get; set; } = default!;
    public Instance GetAppliedInstance() => throw new InvalidOperationException("Cannot call reserved method for RobloxCS transpiler.");
}
