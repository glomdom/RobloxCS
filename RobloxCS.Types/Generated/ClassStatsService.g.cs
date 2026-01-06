#nullable enable
namespace RobloxCS.Types;
[RobloxNative("Stats", RobloxNativeType.Service)]
public static partial class StatsService {
    public static int ContactsCount { get; set; } = default!;
    public static float DataReceiveKbps { get; set; } = default!;
    public static float DataSendKbps { get; set; } = default!;
    public static float FrameTime { get; set; } = default!;
    public static float HeartbeatTime { get; set; } = default!;
    public static int InstanceCount { get; set; } = default!;
    public static bool MemoryTrackingEnabled { get; set; } = default!;
    public static int MovingPrimitivesCount { get; set; } = default!;
    public static float PhysicsReceiveKbps { get; set; } = default!;
    public static float PhysicsSendKbps { get; set; } = default!;
    public static float PhysicsStepTime { get; set; } = default!;
    public static int PrimitivesCount { get; set; } = default!;
    public static float RenderCPUFrameTime { get; set; } = default!;
    public static float RenderGPUFrameTime { get; set; } = default!;
    public static int SceneDrawcallCount { get; set; } = default!;
    public static int SceneTriangleCount { get; set; } = default!;
    public static int ShadowsDrawcallCount { get; set; } = default!;
    public static int ShadowsTriangleCount { get; set; } = default!;
    public static int UI2DDrawcallCount { get; set; } = default!;
    public static int UI2DTriangleCount { get; set; } = default!;
    public static int UI3DDrawcallCount { get; set; } = default!;
    public static int UI3DTriangleCount { get; set; } = default!;
    public static int GetHarmonyQualityLevel() => throw new InvalidOperationException("Cannot call reserved method for RobloxCS transpiler.");
    public static List<object> GetMemoryCategoryNames() => throw new InvalidOperationException("Cannot call reserved method for RobloxCS transpiler.");
    public static List<object> GetMemoryUsageMbAllCategories() => throw new InvalidOperationException("Cannot call reserved method for RobloxCS transpiler.");
    public static float GetMemoryUsageMbForTag() => throw new InvalidOperationException("Cannot call reserved method for RobloxCS transpiler.");
    public static float GetTotalMemoryUsageMb() => throw new InvalidOperationException("Cannot call reserved method for RobloxCS transpiler.");
    public static void ResetHarmonyMemoryTarget() => throw new InvalidOperationException("Cannot call reserved method for RobloxCS transpiler.");
    public static void SetHarmonyMemoryTarget() => throw new InvalidOperationException("Cannot call reserved method for RobloxCS transpiler.");
}
