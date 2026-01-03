#nullable enable
namespace RobloxCS.Types;
[RobloxNative("NetworkSettings", RobloxNativeType.Service)]
public static partial class NetworkSettingsService {
    public static bool HttpProxyEnabled { get; } = default!;
    public static string HttpProxyURL { get; } = default!;
    public static double IncomingReplicationLag { get; } = default!;
    public static bool PrintJoinSizeBreakdown { get; } = default!;
    public static bool PrintPhysicsErrors { get; } = default!;
    public static bool PrintStreamInstanceQuota { get; } = default!;
    public static bool RandomizeJoinInstanceOrder { get; } = default!;
    public static bool RenderStreamedRegions { get; } = default!;
    public static bool ShowActiveAnimationAsset { get; } = default!;
}
