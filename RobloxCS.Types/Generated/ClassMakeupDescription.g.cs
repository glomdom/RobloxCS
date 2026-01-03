#nullable enable
namespace RobloxCS.Types;
[RobloxNative("MakeupDescription", RobloxNativeType.Instance)]
public partial class MakeupDescription : Instance  {
    public int AssetId { get; } = default!;
    public Instance Instance { get; } = default!;
    public Enums.MakeupType MakeupType { get; } = default!;
    public int Order { get; } = default!;
    public Instance GetAppliedInstance() => throw new InvalidOperationException("Cannot call reserved method for RobloxCS transpiler.");
}
