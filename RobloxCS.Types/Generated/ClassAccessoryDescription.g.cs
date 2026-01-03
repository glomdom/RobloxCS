#nullable enable
namespace RobloxCS.Types;
[RobloxNative("AccessoryDescription", RobloxNativeType.Instance)]
public partial class AccessoryDescription : Instance  {
    public Enums.AccessoryType AccessoryType { get; } = default!;
    public int AssetId { get; } = default!;
    public Instance Instance { get; } = default!;
    public bool IsLayered { get; } = default!;
    public int Order { get; } = default!;
    public Vector3 Position { get; } = default!;
    public float Puffiness { get; } = default!;
    public Vector3 Rotation { get; } = default!;
    public Vector3 Scale { get; } = default!;
    public Instance GetAppliedInstance() => throw new InvalidOperationException("Cannot call reserved method for RobloxCS transpiler.");
}
