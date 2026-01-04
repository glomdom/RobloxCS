#nullable enable
namespace RobloxCS.Types;
[RobloxNative("TestService", RobloxNativeType.Service)]
public static partial class TestService {
    public static bool AutoRuns { get; } = default!;
    public static string Description { get; } = default!;
    public static int ErrorCount { get; } = default!;
    public static bool ExecuteWithStudioRun { get; } = default!;
    public static bool IsPhysicsEnvironmentalThrottled { get; } = default!;
    public static bool IsSleepAllowed { get; } = default!;
    public static int NumberOfPlayers { get; } = default!;
    public static double SimulateSecondsLag { get; } = default!;
    public static int TestCount { get; } = default!;
    public static bool ThrottlePhysicsToRealtime { get; } = default!;
    public static double Timeout { get; } = default!;
    public static int WarnCount { get; } = default!;
    public static void Check() => throw new InvalidOperationException("Cannot call reserved method for RobloxCS transpiler.");
    public static void Checkpoint() => throw new InvalidOperationException("Cannot call reserved method for RobloxCS transpiler.");
    public static void Done() => throw new InvalidOperationException("Cannot call reserved method for RobloxCS transpiler.");
    public static void Error() => throw new InvalidOperationException("Cannot call reserved method for RobloxCS transpiler.");
    public static void Fail() => throw new InvalidOperationException("Cannot call reserved method for RobloxCS transpiler.");
    public static void Message() => throw new InvalidOperationException("Cannot call reserved method for RobloxCS transpiler.");
    public static void Require() => throw new InvalidOperationException("Cannot call reserved method for RobloxCS transpiler.");
    public static Dictionary<string, object> ScopeTime() => throw new InvalidOperationException("Cannot call reserved method for RobloxCS transpiler.");
    public static void TakeSnapshot() => throw new InvalidOperationException("Cannot call reserved method for RobloxCS transpiler.");
    public static void Warn() => throw new InvalidOperationException("Cannot call reserved method for RobloxCS transpiler.");
    public static bool isFeatureEnabled() => throw new InvalidOperationException("Cannot call reserved method for RobloxCS transpiler.");
    public static RBXScriptSignal<bool, string, Instance, int> ServerCollectConditionalResult { get; private set; } = null!;
    public static RBXScriptSignal<string, Instance, int> ServerCollectResult { get; private set; } = null!;
}
