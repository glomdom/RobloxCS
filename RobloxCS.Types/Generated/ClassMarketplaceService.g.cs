#nullable enable
namespace RobloxCS.Types;
[RobloxNative("MarketplaceService", RobloxNativeType.Service)]
public static partial class MarketplaceService {
    public static void PromptBulkPurchase() => throw new InvalidOperationException("Cannot call reserved method for RobloxCS transpiler.");
    public static void PromptBundlePurchase() => throw new InvalidOperationException("Cannot call reserved method for RobloxCS transpiler.");
    public static void PromptCancelSubscription() => throw new InvalidOperationException("Cannot call reserved method for RobloxCS transpiler.");
    public static void PromptGamePassPurchase() => throw new InvalidOperationException("Cannot call reserved method for RobloxCS transpiler.");
    public static void PromptPremiumPurchase() => throw new InvalidOperationException("Cannot call reserved method for RobloxCS transpiler.");
    public static void PromptProductPurchase() => throw new InvalidOperationException("Cannot call reserved method for RobloxCS transpiler.");
    public static void PromptPurchase() => throw new InvalidOperationException("Cannot call reserved method for RobloxCS transpiler.");
    public static void PromptSubscriptionPurchase() => throw new InvalidOperationException("Cannot call reserved method for RobloxCS transpiler.");
    public static Instance GetDeveloperProductsAsync() => throw new InvalidOperationException("Cannot call reserved method for RobloxCS transpiler.");
    public static Dictionary<string, object> GetProductInfoAsync() => throw new InvalidOperationException("Cannot call reserved method for RobloxCS transpiler.");
    public static Dictionary<string, object> GetSubscriptionProductInfoAsync() => throw new InvalidOperationException("Cannot call reserved method for RobloxCS transpiler.");
    public static Dictionary<string, object> GetUserSubscriptionDetailsAsync() => throw new InvalidOperationException("Cannot call reserved method for RobloxCS transpiler.");
    public static List<object> GetUserSubscriptionPaymentHistoryAsync() => throw new InvalidOperationException("Cannot call reserved method for RobloxCS transpiler.");
    public static Dictionary<string, object> GetUserSubscriptionStatusAsync() => throw new InvalidOperationException("Cannot call reserved method for RobloxCS transpiler.");
    public static List<object> GetUsersPriceLevelsAsync() => throw new InvalidOperationException("Cannot call reserved method for RobloxCS transpiler.");
    public static bool PlayerOwnsAssetAsync() => throw new InvalidOperationException("Cannot call reserved method for RobloxCS transpiler.");
    public static bool PlayerOwnsBundleAsync() => throw new InvalidOperationException("Cannot call reserved method for RobloxCS transpiler.");
    public static List<object> RankProductsAsync() => throw new InvalidOperationException("Cannot call reserved method for RobloxCS transpiler.");
    public static List<object> RecommendTopProductsAsync() => throw new InvalidOperationException("Cannot call reserved method for RobloxCS transpiler.");
    public static bool UserOwnsGamePassAsync() => throw new InvalidOperationException("Cannot call reserved method for RobloxCS transpiler.");
    public static RBXScriptSignal<Instance, Enums.MarketplaceBulkPurchasePromptStatus, Dictionary<string, object>> PromptBulkPurchaseFinished { get; private set; } = null!;
    public static RBXScriptSignal<Instance, int, bool> PromptBundlePurchaseFinished { get; private set; } = null!;
    public static RBXScriptSignal<Instance, int, bool> PromptGamePassPurchaseFinished { get; private set; } = null!;
    public static RBXScriptSignal PromptPremiumPurchaseFinished { get; private set; } = null!;
    public static RBXScriptSignal<int, int, bool> PromptProductPurchaseFinished { get; private set; } = null!;
    public static RBXScriptSignal<Instance, int, bool> PromptPurchaseFinished { get; private set; } = null!;
    public static RBXScriptSignal<Player, string, bool> PromptSubscriptionPurchaseFinished { get; private set; } = null!;
}
