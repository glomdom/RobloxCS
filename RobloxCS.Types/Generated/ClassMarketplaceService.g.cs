#nullable enable
namespace RobloxCS.Types;
[RobloxNative("MarketplaceService", RobloxNativeType.Service)]
public static partial class MarketplaceService {
    public static void PromptBulkPurchase() => ThrowHelper.ThrowTranspiledMethod();
    public static void PromptBundlePurchase() => ThrowHelper.ThrowTranspiledMethod();
    public static void PromptCancelSubscription() => ThrowHelper.ThrowTranspiledMethod();
    public static void PromptGamePassPurchase() => ThrowHelper.ThrowTranspiledMethod();
    public static void PromptPremiumPurchase() => ThrowHelper.ThrowTranspiledMethod();
    public static void PromptProductPurchase() => ThrowHelper.ThrowTranspiledMethod();
    public static void PromptPurchase() => ThrowHelper.ThrowTranspiledMethod();
    public static void PromptSubscriptionPurchase() => ThrowHelper.ThrowTranspiledMethod();
    public static Instance GetDeveloperProductsAsync() => ThrowHelper.ThrowTranspiledMethod();
    public static Dictionary<string, object> GetProductInfoAsync() => ThrowHelper.ThrowTranspiledMethod();
    public static Dictionary<string, object> GetSubscriptionProductInfoAsync() => ThrowHelper.ThrowTranspiledMethod();
    public static Dictionary<string, object> GetUserSubscriptionDetailsAsync() => ThrowHelper.ThrowTranspiledMethod();
    public static List<object> GetUserSubscriptionPaymentHistoryAsync() => ThrowHelper.ThrowTranspiledMethod();
    public static Dictionary<string, object> GetUserSubscriptionStatusAsync() => ThrowHelper.ThrowTranspiledMethod();
    public static List<object> GetUsersPriceLevelsAsync() => ThrowHelper.ThrowTranspiledMethod();
    public static bool PlayerOwnsAssetAsync() => ThrowHelper.ThrowTranspiledMethod();
    public static bool PlayerOwnsBundleAsync() => ThrowHelper.ThrowTranspiledMethod();
    public static List<object> RankProductsAsync() => ThrowHelper.ThrowTranspiledMethod();
    public static List<object> RecommendTopProductsAsync() => ThrowHelper.ThrowTranspiledMethod();
    public static bool UserOwnsGamePassAsync() => ThrowHelper.ThrowTranspiledMethod();
    public static RBXScriptSignal<Instance, Enums.MarketplaceBulkPurchasePromptStatus, Dictionary<string, object>> PromptBulkPurchaseFinished { get; private set; } = null!;
    public static RBXScriptSignal<Instance, int, bool> PromptBundlePurchaseFinished { get; private set; } = null!;
    public static RBXScriptSignal<Instance, int, bool> PromptGamePassPurchaseFinished { get; private set; } = null!;
    public static RBXScriptSignal PromptPremiumPurchaseFinished { get; private set; } = null!;
    public static RBXScriptSignal<int, int, bool> PromptProductPurchaseFinished { get; private set; } = null!;
    public static RBXScriptSignal<Instance, int, bool> PromptPurchaseFinished { get; private set; } = null!;
    public static RBXScriptSignal<Player, string, bool> PromptSubscriptionPurchaseFinished { get; private set; } = null!;
    public static Func<Dictionary<string, object>> ProcessReceipt { get; set; } = default!;
}
