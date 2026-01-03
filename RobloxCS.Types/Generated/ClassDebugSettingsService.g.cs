#nullable enable
namespace RobloxCS.Types;
[RobloxNative("DebugSettings", RobloxNativeType.Service)]
public static partial class DebugSettingsService {
    public static int DataModel { get; } = default!;
    public static int InstanceCount { get; } = default!;
    public static bool IsScriptStackTracingEnabled { get; } = default!;
    public static int JobCount { get; } = default!;
    public static int PlayerCount { get; } = default!;
    public static bool ReportSoundWarnings { get; } = default!;
    public static string RobloxVersion { get; } = default!;
    public static Enums.TickCountSampleMethod TickCountPreciseOverride { get; } = default!;
}
