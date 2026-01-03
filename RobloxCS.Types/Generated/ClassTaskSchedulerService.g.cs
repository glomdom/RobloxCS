#nullable enable
namespace RobloxCS.Types;
[RobloxNative("TaskScheduler", RobloxNativeType.Service)]
public static partial class TaskSchedulerService {
    public static double SchedulerDutyCycle { get; } = default!;
    public static double SchedulerRate { get; } = default!;
    public static Enums.ThreadPoolConfig ThreadPoolConfig { get; } = default!;
    public static int ThreadPoolSize { get; } = default!;
}
