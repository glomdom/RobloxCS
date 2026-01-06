#nullable enable
namespace RobloxCS.Types;
[RobloxNative("InputContext", RobloxNativeType.Instance)]
public partial class InputContext : Instance  {
    public bool Enabled { get; set; } = default!;
    public int Priority { get; set; } = default!;
    public bool Sink { get; set; } = default!;
}
