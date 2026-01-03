#nullable enable
namespace RobloxCS.Types;
[RobloxNative("ContentProvider", RobloxNativeType.Service)]
public static partial class ContentProviderService {
    public static string BaseUrl { get; } = default!;
    public static int RequestQueueSize { get; } = default!;
    public static Enums.AssetFetchStatus GetAssetFetchStatus() => throw new InvalidOperationException("Cannot call reserved method for RobloxCS transpiler.");
    public static RBXScriptSignal GetAssetFetchStatusChangedSignal() => throw new InvalidOperationException("Cannot call reserved method for RobloxCS transpiler.");
    public static List<object> ListEncryptedAssets() => throw new InvalidOperationException("Cannot call reserved method for RobloxCS transpiler.");
    public static void RegisterDefaultEncryptionKey() => throw new InvalidOperationException("Cannot call reserved method for RobloxCS transpiler.");
    public static void RegisterDefaultSessionKey() => throw new InvalidOperationException("Cannot call reserved method for RobloxCS transpiler.");
    public static void RegisterEncryptedAsset() => throw new InvalidOperationException("Cannot call reserved method for RobloxCS transpiler.");
    public static void RegisterSessionEncryptedAsset() => throw new InvalidOperationException("Cannot call reserved method for RobloxCS transpiler.");
    public static void UnregisterDefaultEncryptionKey() => throw new InvalidOperationException("Cannot call reserved method for RobloxCS transpiler.");
    public static void UnregisterEncryptedAsset() => throw new InvalidOperationException("Cannot call reserved method for RobloxCS transpiler.");
    public static void PreloadAsync() => throw new InvalidOperationException("Cannot call reserved method for RobloxCS transpiler.");
}
