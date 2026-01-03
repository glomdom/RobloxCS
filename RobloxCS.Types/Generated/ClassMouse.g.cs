#nullable enable
namespace RobloxCS.Types;
[RobloxNative("Mouse", RobloxNativeType.Instance)]
public partial class Mouse : Instance  {
    public CFrame Hit { get; } = default!;
    public string Icon { get; } = default!;
    public string IconContent { get; } = default!;
    public CFrame Origin { get; } = default!;
    public BasePart Target { get; } = default!;
    public Instance TargetFilter { get; } = default!;
    public Enums.NormalId TargetSurface { get; } = default!;
    public Ray UnitRay { get; } = default!;
    public int ViewSizeX { get; } = default!;
    public int ViewSizeY { get; } = default!;
    public int X { get; } = default!;
    public int Y { get; } = default!;
}
