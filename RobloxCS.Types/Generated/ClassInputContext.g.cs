#nullable enable
namespace RobloxCS.Types;
[RobloxNative("InputContext", RobloxNativeType.Instance)]
public partial class InputContext : Instance  {
    public bool Enabled { get; } = default!;
    public int Priority { get; } = default!;
    public bool Sink { get; } = default!;
}
