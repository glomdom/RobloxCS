#nullable enable
namespace RobloxCS.Types;
[RobloxNative("RenderSettings", RobloxNativeType.Service)]
public static partial class RenderSettingsService {
    public static int AutoFRMLevel { get; } = default!;
    public static bool EagerBulkExecution { get; } = default!;
    public static Enums.QualityLevel EditQualityLevel { get; } = default!;
    public static bool EnableVRMode { get; } = default!;
    public static bool ExportMergeByMaterial { get; } = default!;
    public static Enums.FramerateManagerMode FrameRateManager { get; } = default!;
    public static Enums.GraphicsMode GraphicsMode { get; } = default!;
    public static int MeshCacheSize { get; } = default!;
    public static Enums.MeshPartDetailLevel MeshPartDetailLevel { get; } = default!;
    public static Enums.QualityLevel QualityLevel { get; } = default!;
    public static bool ReloadAssets { get; } = default!;
    public static bool RenderCSGTrianglesDebug { get; } = default!;
    public static bool ShowBoundingBoxes { get; } = default!;
    public static Enums.ViewMode ViewMode { get; } = default!;
    public static int GetMaxQualityLevel() => throw new InvalidOperationException("Cannot call reserved method for RobloxCS transpiler.");
}
