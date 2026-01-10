#nullable enable
namespace RobloxCS.Types;
[RobloxNative("ExperienceNotificationService", RobloxNativeType.Service)]
public static partial class ExperienceNotificationService {
    public static void PromptOptIn() => ThrowHelper.ThrowTranspiledMethod();
    public static bool CanPromptOptInAsync() => ThrowHelper.ThrowTranspiledMethod();
    public static RBXScriptSignal OptInPromptClosed { get; private set; } = null!;
}
