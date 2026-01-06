#nullable enable
namespace RobloxCS.Types;
[RobloxNative("StudioService", RobloxNativeType.Service)]
public static partial class StudioService {
    public static Instance ActiveScript { get; set; } = default!;
    public static bool DraggerSolveConstraints { get; set; } = default!;
    public static float GridSize { get; set; } = default!;
    public static float RotateIncrement { get; set; } = default!;
    public static bool ShowConstraintDetails { get; set; } = default!;
    public static string StudioLocaleId { get; set; } = default!;
    public static bool UseLocalSpace { get; set; } = default!;
}
