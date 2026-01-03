#nullable enable
namespace RobloxCS.Types;
[RobloxNative("StudioService", RobloxNativeType.Service)]
public static partial class StudioService {
    public static Instance ActiveScript { get; } = default!;
    public static bool DraggerSolveConstraints { get; } = default!;
    public static float GridSize { get; } = default!;
    public static float RotateIncrement { get; } = default!;
    public static bool ShowConstraintDetails { get; } = default!;
    public static string StudioLocaleId { get; } = default!;
    public static bool UseLocalSpace { get; } = default!;
}
