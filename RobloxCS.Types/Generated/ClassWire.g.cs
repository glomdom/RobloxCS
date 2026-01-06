#nullable enable
namespace RobloxCS.Types;
[RobloxNative("Wire", RobloxNativeType.Instance)]
public partial class Wire : Instance  {
    public bool Connected { get; set; } = default!;
    public Instance SourceInstance { get; set; } = default!;
    public string SourceName { get; set; } = default!;
    public Instance TargetInstance { get; set; } = default!;
    public string TargetName { get; set; } = default!;
}
