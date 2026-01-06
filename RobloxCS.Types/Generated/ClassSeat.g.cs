#nullable enable
namespace RobloxCS.Types;
[RobloxNative("Seat", RobloxNativeType.Instance)]
public partial class Seat : Part  {
    public bool Disabled { get; set; } = default!;
    public Humanoid Occupant { get; set; } = default!;
    public void Sit() => throw new InvalidOperationException("Cannot call reserved method for RobloxCS transpiler.");
}
