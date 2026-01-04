#nullable enable
namespace RobloxCS.Types;
[RobloxNative("AvatarEditorService", RobloxNativeType.Service)]
public static partial class AvatarEditorService {
    public static Enums.AccessoryType GetAccessoryType() => throw new InvalidOperationException("Cannot call reserved method for RobloxCS transpiler.");
    public static void PromptAllowInventoryReadAccess() => throw new InvalidOperationException("Cannot call reserved method for RobloxCS transpiler.");
    public static void PromptCreateOutfit() => throw new InvalidOperationException("Cannot call reserved method for RobloxCS transpiler.");
    public static void PromptDeleteOutfit() => throw new InvalidOperationException("Cannot call reserved method for RobloxCS transpiler.");
    public static void PromptRenameOutfit() => throw new InvalidOperationException("Cannot call reserved method for RobloxCS transpiler.");
    public static void PromptSaveAvatar() => throw new InvalidOperationException("Cannot call reserved method for RobloxCS transpiler.");
    public static void PromptSetFavorite() => throw new InvalidOperationException("Cannot call reserved method for RobloxCS transpiler.");
    public static void PromptUpdateOutfit() => throw new InvalidOperationException("Cannot call reserved method for RobloxCS transpiler.");
    public static HumanoidDescription CheckApplyDefaultClothingAsync() => throw new InvalidOperationException("Cannot call reserved method for RobloxCS transpiler.");
    public static HumanoidDescription ConformToAvatarRulesAsync() => throw new InvalidOperationException("Cannot call reserved method for RobloxCS transpiler.");
    public static Dictionary<string, object> GetAvatarRulesAsync() => throw new InvalidOperationException("Cannot call reserved method for RobloxCS transpiler.");
    public static List<object> GetBatchItemDetailsAsync() => throw new InvalidOperationException("Cannot call reserved method for RobloxCS transpiler.");
    public static bool GetFavoriteAsync() => throw new InvalidOperationException("Cannot call reserved method for RobloxCS transpiler.");
    public static InventoryPages GetInventoryAsync() => throw new InvalidOperationException("Cannot call reserved method for RobloxCS transpiler.");
    public static Dictionary<string, object> GetItemDetailsAsync() => throw new InvalidOperationException("Cannot call reserved method for RobloxCS transpiler.");
    public static Dictionary<string, object> GetOutfitDetailsAsync() => throw new InvalidOperationException("Cannot call reserved method for RobloxCS transpiler.");
    public static OutfitPages GetOutfitsAsync() => throw new InvalidOperationException("Cannot call reserved method for RobloxCS transpiler.");
    public static List<object> GetRecommendedAssetsAsync() => throw new InvalidOperationException("Cannot call reserved method for RobloxCS transpiler.");
    public static List<object> GetRecommendedBundlesAsync() => throw new InvalidOperationException("Cannot call reserved method for RobloxCS transpiler.");
    public static CatalogPages SearchCatalogAsync() => throw new InvalidOperationException("Cannot call reserved method for RobloxCS transpiler.");
    public static RBXScriptSignal<Enums.AvatarPromptResult> PromptAllowInventoryReadAccessCompleted { get; private set; } = null!;
    public static RBXScriptSignal<Enums.AvatarPromptResult, object> PromptCreateOutfitCompleted { get; private set; } = null!;
    public static RBXScriptSignal<Enums.AvatarPromptResult> PromptDeleteOutfitCompleted { get; private set; } = null!;
    public static RBXScriptSignal<Enums.AvatarPromptResult> PromptRenameOutfitCompleted { get; private set; } = null!;
    public static RBXScriptSignal<Enums.AvatarPromptResult, HumanoidDescription> PromptSaveAvatarCompleted { get; private set; } = null!;
    public static RBXScriptSignal<Enums.AvatarPromptResult> PromptSetFavoriteCompleted { get; private set; } = null!;
    public static RBXScriptSignal<Enums.AvatarPromptResult> PromptUpdateOutfitCompleted { get; private set; } = null!;
}
