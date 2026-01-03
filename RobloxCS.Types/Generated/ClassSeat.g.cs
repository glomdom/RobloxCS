#nullable enable
namespace RobloxCS.Types;
[RobloxNative("Seat", RobloxNativeType.Instance)]
public partial class Seat : Part  {
    public bool Disabled { get; } = default!;
    public Humanoid Occupant { get; } = default!;
    public void Sit() => throw new InvalidOperationException("Cannot call reserved method for RobloxCS transpiler.");
}
