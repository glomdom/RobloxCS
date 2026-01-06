#nullable enable
namespace RobloxCS.Types;
[RobloxNative("Constraint", RobloxNativeType.Instance)]
public partial class Constraint : Instance  {
    public bool Active { get; set; } = default!;
    public Attachment Attachment0 { get; set; } = default!;
    public Attachment Attachment1 { get; set; } = default!;
    public BrickColor Color { get; set; } = default!;
    public bool Enabled { get; set; } = default!;
    public bool Visible { get; set; } = default!;
}
