#nullable enable
namespace RobloxCS.Types;
[RobloxNative("DraggerService", RobloxNativeType.Service)]
public static partial class DraggerService {
    public static bool AlignDraggedObjects { get; set; } = default!;
    public static bool AngleSnapEnabled { get; set; } = default!;
    public static float AngleSnapIncrement { get; set; } = default!;
    public static bool AnimateHover { get; set; } = default!;
    public static bool CollisionsEnabled { get; set; } = default!;
    public static Enums.DraggerCoordinateSpace DraggerCoordinateSpace { get; set; } = default!;
    public static Enums.DraggerMovementMode DraggerMovementMode { get; set; } = default!;
    public static Color3 GeometrySnapColor { get; set; } = default!;
    public static float HoverAnimateFrequency { get; set; } = default!;
    public static float HoverThickness { get; set; } = default!;
    public static bool JointsEnabled { get; set; } = default!;
    public static bool LinearSnapEnabled { get; set; } = default!;
    public static float LinearSnapIncrement { get; set; } = default!;
    public static bool ShowHover { get; set; } = default!;
    public static bool ShowPivotIndicator { get; set; } = default!;
}
