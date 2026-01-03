#nullable enable
namespace RobloxCS.Types;
[RobloxNative("Stats", RobloxNativeType.Service)]
public static partial class StatsService {
    public static int ContactsCount { get; } = default!;
    public static float DataReceiveKbps { get; } = default!;
    public static float DataSendKbps { get; } = default!;
    public static float FrameTime { get; } = default!;
    public static float HeartbeatTime { get; } = default!;
    public static int InstanceCount { get; } = default!;
    public static bool MemoryTrackingEnabled { get; } = default!;
    public static int MovingPrimitivesCount { get; } = default!;
    public static float PhysicsReceiveKbps { get; } = default!;
    public static float PhysicsSendKbps { get; } = default!;
    public static float PhysicsStepTime { get; } = default!;
    public static int PrimitivesCount { get; } = default!;
    public static float RenderCPUFrameTime { get; } = default!;
    public static float RenderGPUFrameTime { get; } = default!;
    public static int SceneDrawcallCount { get; } = default!;
    public static int SceneTriangleCount { get; } = default!;
    public static int ShadowsDrawcallCount { get; } = default!;
    public static int ShadowsTriangleCount { get; } = default!;
    public static int UI2DDrawcallCount { get; } = default!;
    public static int UI2DTriangleCount { get; } = default!;
    public static int UI3DDrawcallCount { get; } = default!;
    public static int UI3DTriangleCount { get; } = default!;
    public static int GetHarmonyQualityLevel() => throw new InvalidOperationException("Cannot call reserved method for RobloxCS transpiler.");
    public static List<object> GetMemoryCategoryNames() => throw new InvalidOperationException("Cannot call reserved method for RobloxCS transpiler.");
    public static List<object> GetMemoryUsageMbAllCategories() => throw new InvalidOperationException("Cannot call reserved method for RobloxCS transpiler.");
    public static float GetMemoryUsageMbForTag() => throw new InvalidOperationException("Cannot call reserved method for RobloxCS transpiler.");
    public static float GetTotalMemoryUsageMb() => throw new InvalidOperationException("Cannot call reserved method for RobloxCS transpiler.");
    public static void ResetHarmonyMemoryTarget() => throw new InvalidOperationException("Cannot call reserved method for RobloxCS transpiler.");
    public static void SetHarmonyMemoryTarget() => throw new InvalidOperationException("Cannot call reserved method for RobloxCS transpiler.");
}
