#nullable enable
namespace RobloxCS.Types;
[RobloxNative("Wire", RobloxNativeType.Instance)]
public partial class Wire : Instance  {
    public bool Connected { get; } = default!;
    public Instance SourceInstance { get; } = default!;
    public string SourceName { get; } = default!;
    public Instance TargetInstance { get; } = default!;
    public string TargetName { get; } = default!;
}
