#nullable enable
namespace RobloxCS.Types;
[RobloxNative("AvatarCreationService", RobloxNativeType.Service)]
public static partial class AvatarCreationService {
    public static Dictionary<string, object> GetValidationRules() => ThrowHelper.ThrowTranspiledMethod();
    public static string AutoSetupAvatarAsync() => ThrowHelper.ThrowTranspiledMethod();
    public static string AutoSetupAvatarNewAsync() => ThrowHelper.ThrowTranspiledMethod();
    public static string GenerateAvatar2DPreviewAsync() => ThrowHelper.ThrowTranspiledMethod();
    public static string GenerateAvatarAsync() => ThrowHelper.ThrowTranspiledMethod();
    public static List<object> GetBatchTokenDetailsAsync() => ThrowHelper.ThrowTranspiledMethod();
    public static EditableImage LoadAvatar2DPreviewAsync() => ThrowHelper.ThrowTranspiledMethod();
    public static HumanoidDescription LoadGeneratedAvatarAsync() => ThrowHelper.ThrowTranspiledMethod();
    public static void PrepareAvatarForPreviewAsync() => ThrowHelper.ThrowTranspiledMethod();
    public static LuaTuple PromptCreateAvatarAssetAsync() => ThrowHelper.ThrowTranspiledMethod();
    public static LuaTuple PromptCreateAvatarAsync() => ThrowHelper.ThrowTranspiledMethod();
    public static string PromptSelectAvatarGenerationImageAsync() => ThrowHelper.ThrowTranspiledMethod();
    public static LuaTuple RequestAvatarGenerationSessionAsync() => ThrowHelper.ThrowTranspiledMethod();
    public static LuaTuple ValidateUGCAccessoryAsync() => ThrowHelper.ThrowTranspiledMethod();
    public static LuaTuple ValidateUGCBodyPartAsync() => ThrowHelper.ThrowTranspiledMethod();
    public static LuaTuple ValidateUGCFullBodyAsync() => ThrowHelper.ThrowTranspiledMethod();
    public static RBXScriptSignal<int, Enums.ModerationStatus> AvatarAssetModerationCompleted { get; private set; } = null!;
    public static RBXScriptSignal<int, Enums.ModerationStatus> AvatarModerationCompleted { get; private set; } = null!;
}
