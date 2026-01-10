#nullable enable
namespace RobloxCS.Types;
[RobloxNative("Model", RobloxNativeType.Instance)]
public partial class Model : PVInstance  {
    public Enums.ModelStreamingMode ModelStreamingMode { get; set; } = default!;
    public BasePart PrimaryPart { get; set; } = default!;
    public float Scale { get; set; } = default!;
    public CFrame WorldPivot { get; set; } = default!;
    public void AddPersistentPlayer() => ThrowHelper.ThrowTranspiledMethod();
    public (CFrame, Vector3) GetBoundingBox() => ThrowHelper.ThrowTranspiledMethod();
    public Vector3 GetExtentsSize() => ThrowHelper.ThrowTranspiledMethod();
    public List<Instance> GetPersistentPlayers() => ThrowHelper.ThrowTranspiledMethod();
    public float GetScale() => ThrowHelper.ThrowTranspiledMethod();
    public void MoveTo() => ThrowHelper.ThrowTranspiledMethod();
    public void RemovePersistentPlayer() => ThrowHelper.ThrowTranspiledMethod();
    public void ScaleTo() => ThrowHelper.ThrowTranspiledMethod();
    public void TranslateBy() => ThrowHelper.ThrowTranspiledMethod();
}
