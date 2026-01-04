#nullable enable
namespace RobloxCS.Types;
[RobloxNative("AvatarCreationService", RobloxNativeType.Service)]
public static partial class AvatarCreationService {
    public static Dictionary<string, object> GetValidationRules() => throw new InvalidOperationException("Cannot call reserved method for RobloxCS transpiler.");
    public static string AutoSetupAvatarAsync() => throw new InvalidOperationException("Cannot call reserved method for RobloxCS transpiler.");
    public static string AutoSetupAvatarNewAsync() => throw new InvalidOperationException("Cannot call reserved method for RobloxCS transpiler.");
    public static string GenerateAvatar2DPreviewAsync() => throw new InvalidOperationException("Cannot call reserved method for RobloxCS transpiler.");
    public static string GenerateAvatarAsync() => throw new InvalidOperationException("Cannot call reserved method for RobloxCS transpiler.");
    public static List<object> GetBatchTokenDetailsAsync() => throw new InvalidOperationException("Cannot call reserved method for RobloxCS transpiler.");
    public static EditableImage LoadAvatar2DPreviewAsync() => throw new InvalidOperationException("Cannot call reserved method for RobloxCS transpiler.");
    public static HumanoidDescription LoadGeneratedAvatarAsync() => throw new InvalidOperationException("Cannot call reserved method for RobloxCS transpiler.");
    public static void PrepareAvatarForPreviewAsync() => throw new InvalidOperationException("Cannot call reserved method for RobloxCS transpiler.");
    public static LuaTuple PromptCreateAvatarAssetAsync() => throw new InvalidOperationException("Cannot call reserved method for RobloxCS transpiler.");
    public static LuaTuple PromptCreateAvatarAsync() => throw new InvalidOperationException("Cannot call reserved method for RobloxCS transpiler.");
    public static string PromptSelectAvatarGenerationImageAsync() => throw new InvalidOperationException("Cannot call reserved method for RobloxCS transpiler.");
    public static LuaTuple RequestAvatarGenerationSessionAsync() => throw new InvalidOperationException("Cannot call reserved method for RobloxCS transpiler.");
    public static LuaTuple ValidateUGCAccessoryAsync() => throw new InvalidOperationException("Cannot call reserved method for RobloxCS transpiler.");
    public static LuaTuple ValidateUGCBodyPartAsync() => throw new InvalidOperationException("Cannot call reserved method for RobloxCS transpiler.");
    public static LuaTuple ValidateUGCFullBodyAsync() => throw new InvalidOperationException("Cannot call reserved method for RobloxCS transpiler.");
    public static RBXScriptSignal<int, Enums.ModerationStatus> AvatarAssetModerationCompleted { get; private set; } = null!;
    public static RBXScriptSignal<int, Enums.ModerationStatus> AvatarModerationCompleted { get; private set; } = null!;
}
