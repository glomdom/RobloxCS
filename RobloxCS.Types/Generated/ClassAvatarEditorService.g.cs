#nullable enable
namespace RobloxCS.Types;
[RobloxNative("AvatarEditorService", RobloxNativeType.Service)]
public static partial class AvatarEditorService {
    public static Enums.AccessoryType GetAccessoryType() => ThrowHelper.ThrowTranspiledMethod();
    public static void PromptAllowInventoryReadAccess() => ThrowHelper.ThrowTranspiledMethod();
    public static void PromptCreateOutfit() => ThrowHelper.ThrowTranspiledMethod();
    public static void PromptDeleteOutfit() => ThrowHelper.ThrowTranspiledMethod();
    public static void PromptRenameOutfit() => ThrowHelper.ThrowTranspiledMethod();
    public static void PromptSaveAvatar() => ThrowHelper.ThrowTranspiledMethod();
    public static void PromptSetFavorite() => ThrowHelper.ThrowTranspiledMethod();
    public static void PromptUpdateOutfit() => ThrowHelper.ThrowTranspiledMethod();
    public static HumanoidDescription CheckApplyDefaultClothingAsync() => ThrowHelper.ThrowTranspiledMethod();
    public static HumanoidDescription ConformToAvatarRulesAsync() => ThrowHelper.ThrowTranspiledMethod();
    public static Dictionary<string, object> GetAvatarRulesAsync() => ThrowHelper.ThrowTranspiledMethod();
    public static List<object> GetBatchItemDetailsAsync() => ThrowHelper.ThrowTranspiledMethod();
    public static bool GetFavoriteAsync() => ThrowHelper.ThrowTranspiledMethod();
    public static InventoryPages GetInventoryAsync() => ThrowHelper.ThrowTranspiledMethod();
    public static Dictionary<string, object> GetItemDetailsAsync() => ThrowHelper.ThrowTranspiledMethod();
    public static Dictionary<string, object> GetOutfitDetailsAsync() => ThrowHelper.ThrowTranspiledMethod();
    public static OutfitPages GetOutfitsAsync() => ThrowHelper.ThrowTranspiledMethod();
    public static List<object> GetRecommendedAssetsAsync() => ThrowHelper.ThrowTranspiledMethod();
    public static List<object> GetRecommendedBundlesAsync() => ThrowHelper.ThrowTranspiledMethod();
    public static CatalogPages SearchCatalogAsync() => ThrowHelper.ThrowTranspiledMethod();
    public static RBXScriptSignal<Enums.AvatarPromptResult> PromptAllowInventoryReadAccessCompleted { get; private set; } = null!;
    public static RBXScriptSignal<Enums.AvatarPromptResult, object> PromptCreateOutfitCompleted { get; private set; } = null!;
    public static RBXScriptSignal<Enums.AvatarPromptResult> PromptDeleteOutfitCompleted { get; private set; } = null!;
    public static RBXScriptSignal<Enums.AvatarPromptResult> PromptRenameOutfitCompleted { get; private set; } = null!;
    public static RBXScriptSignal<Enums.AvatarPromptResult, HumanoidDescription> PromptSaveAvatarCompleted { get; private set; } = null!;
    public static RBXScriptSignal<Enums.AvatarPromptResult> PromptSetFavoriteCompleted { get; private set; } = null!;
    public static RBXScriptSignal<Enums.AvatarPromptResult> PromptUpdateOutfitCompleted { get; private set; } = null!;
}
