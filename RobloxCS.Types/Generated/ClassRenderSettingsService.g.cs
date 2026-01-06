#nullable enable
namespace RobloxCS.Types;
[RobloxNative("RenderSettings", RobloxNativeType.Service)]
public static partial class RenderSettingsService {
    public static int AutoFRMLevel { get; set; } = default!;
    public static bool EagerBulkExecution { get; set; } = default!;
    public static Enums.QualityLevel EditQualityLevel { get; set; } = default!;
    public static bool EnableVRMode { get; set; } = default!;
    public static bool ExportMergeByMaterial { get; set; } = default!;
    public static Enums.FramerateManagerMode FrameRateManager { get; set; } = default!;
    public static Enums.GraphicsMode GraphicsMode { get; set; } = default!;
    public static int MeshCacheSize { get; set; } = default!;
    public static Enums.MeshPartDetailLevel MeshPartDetailLevel { get; set; } = default!;
    public static Enums.QualityLevel QualityLevel { get; set; } = default!;
    public static bool ReloadAssets { get; set; } = default!;
    public static bool RenderCSGTrianglesDebug { get; set; } = default!;
    public static bool ShowBoundingBoxes { get; set; } = default!;
    public static Enums.ViewMode ViewMode { get; set; } = default!;
    public static int GetMaxQualityLevel() => throw new InvalidOperationException("Cannot call reserved method for RobloxCS transpiler.");
}
