#nullable enable
namespace RobloxCS.Types;
[RobloxNative("Constraint", RobloxNativeType.Instance)]
public partial class Constraint : Instance  {
    public bool Active { get; } = default!;
    public Attachment Attachment0 { get; } = default!;
    public Attachment Attachment1 { get; } = default!;
    public BrickColor Color { get; } = default!;
    public bool Enabled { get; } = default!;
    public bool Visible { get; } = default!;
}
