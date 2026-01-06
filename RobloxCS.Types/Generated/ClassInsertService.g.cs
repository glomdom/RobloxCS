#nullable enable
namespace RobloxCS.Types;
[RobloxNative("InsertService", RobloxNativeType.Service)]
public static partial class InsertService {
    public static bool AllowClientInsertModels { get; set; } = default!;
    public static MeshPart CreateMeshPartAsync() => throw new InvalidOperationException("Cannot call reserved method for RobloxCS transpiler.");
    public static List<object> GetFreeDecalsAsync() => throw new InvalidOperationException("Cannot call reserved method for RobloxCS transpiler.");
    public static List<object> GetFreeModelsAsync() => throw new InvalidOperationException("Cannot call reserved method for RobloxCS transpiler.");
    public static int GetLatestAssetVersionAsync() => throw new InvalidOperationException("Cannot call reserved method for RobloxCS transpiler.");
    public static Instance LoadAsset() => throw new InvalidOperationException("Cannot call reserved method for RobloxCS transpiler.");
    public static Instance LoadAssetVersion() => throw new InvalidOperationException("Cannot call reserved method for RobloxCS transpiler.");
}
