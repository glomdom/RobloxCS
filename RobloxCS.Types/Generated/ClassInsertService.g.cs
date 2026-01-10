#nullable enable
namespace RobloxCS.Types;
[RobloxNative("InsertService", RobloxNativeType.Service)]
public static partial class InsertService {
    public static bool AllowClientInsertModels { get; set; } = default!;
    public static MeshPart CreateMeshPartAsync() => ThrowHelper.ThrowTranspiledMethod();
    public static List<object> GetFreeDecalsAsync() => ThrowHelper.ThrowTranspiledMethod();
    public static List<object> GetFreeModelsAsync() => ThrowHelper.ThrowTranspiledMethod();
    public static int GetLatestAssetVersionAsync() => ThrowHelper.ThrowTranspiledMethod();
    public static Instance LoadAsset() => ThrowHelper.ThrowTranspiledMethod();
    public static Instance LoadAssetVersion() => ThrowHelper.ThrowTranspiledMethod();
}
