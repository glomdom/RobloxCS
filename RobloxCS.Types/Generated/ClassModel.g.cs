#nullable enable
namespace RobloxCS.Types;
[RobloxNative("Model", RobloxNativeType.Instance)]
public partial class Model : PVInstance  {
    public Enums.ModelStreamingMode ModelStreamingMode { get; set; } = default!;
    public BasePart PrimaryPart { get; set; } = default!;
    public float Scale { get; set; } = default!;
    public CFrame WorldPivot { get; set; } = default!;
    public void AddPersistentPlayer() => throw new InvalidOperationException("Cannot call reserved method for RobloxCS transpiler.");
    public (CFrame, Vector3) GetBoundingBox() => throw new InvalidOperationException("Cannot call reserved method for RobloxCS transpiler.");
    public Vector3 GetExtentsSize() => throw new InvalidOperationException("Cannot call reserved method for RobloxCS transpiler.");
    public List<Instance> GetPersistentPlayers() => throw new InvalidOperationException("Cannot call reserved method for RobloxCS transpiler.");
    public float GetScale() => throw new InvalidOperationException("Cannot call reserved method for RobloxCS transpiler.");
    public void MoveTo() => throw new InvalidOperationException("Cannot call reserved method for RobloxCS transpiler.");
    public void RemovePersistentPlayer() => throw new InvalidOperationException("Cannot call reserved method for RobloxCS transpiler.");
    public void ScaleTo() => throw new InvalidOperationException("Cannot call reserved method for RobloxCS transpiler.");
    public void TranslateBy() => throw new InvalidOperationException("Cannot call reserved method for RobloxCS transpiler.");
}
