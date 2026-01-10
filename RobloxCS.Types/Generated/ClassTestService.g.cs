#nullable enable
namespace RobloxCS.Types;
[RobloxNative("TestService", RobloxNativeType.Service)]
public static partial class TestService {
    public static bool AutoRuns { get; set; } = default!;
    public static string Description { get; set; } = default!;
    public static int ErrorCount { get; set; } = default!;
    public static bool ExecuteWithStudioRun { get; set; } = default!;
    public static bool IsPhysicsEnvironmentalThrottled { get; set; } = default!;
    public static bool IsSleepAllowed { get; set; } = default!;
    public static int NumberOfPlayers { get; set; } = default!;
    public static double SimulateSecondsLag { get; set; } = default!;
    public static int TestCount { get; set; } = default!;
    public static bool ThrottlePhysicsToRealtime { get; set; } = default!;
    public static double Timeout { get; set; } = default!;
    public static int WarnCount { get; set; } = default!;
    public static void Check() => ThrowHelper.ThrowTranspiledMethod();
    public static void Checkpoint() => ThrowHelper.ThrowTranspiledMethod();
    public static void Done() => ThrowHelper.ThrowTranspiledMethod();
    public static void Error() => ThrowHelper.ThrowTranspiledMethod();
    public static void Fail() => ThrowHelper.ThrowTranspiledMethod();
    public static void Message() => ThrowHelper.ThrowTranspiledMethod();
    public static void Require() => ThrowHelper.ThrowTranspiledMethod();
    public static Dictionary<string, object> ScopeTime() => ThrowHelper.ThrowTranspiledMethod();
    public static void TakeSnapshot() => ThrowHelper.ThrowTranspiledMethod();
    public static void Warn() => ThrowHelper.ThrowTranspiledMethod();
    public static bool isFeatureEnabled() => ThrowHelper.ThrowTranspiledMethod();
    public static RBXScriptSignal<bool, string, Instance, int> ServerCollectConditionalResult { get; private set; } = null!;
    public static RBXScriptSignal<string, Instance, int> ServerCollectResult { get; private set; } = null!;
}
