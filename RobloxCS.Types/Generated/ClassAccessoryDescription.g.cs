#nullable enable
namespace RobloxCS.Types;
[RobloxNative("AccessoryDescription", RobloxNativeType.Instance)]
public partial class AccessoryDescription : Instance  {
    public Enums.AccessoryType AccessoryType { get; set; } = default!;
    public int AssetId { get; set; } = default!;
    public Instance Instance { get; set; } = default!;
    public bool IsLayered { get; set; } = default!;
    public int Order { get; set; } = default!;
    public Vector3 Position { get; set; } = default!;
    public float Puffiness { get; set; } = default!;
    public Vector3 Rotation { get; set; } = default!;
    public Vector3 Scale { get; set; } = default!;
    public Instance GetAppliedInstance() => throw new InvalidOperationException("Cannot call reserved method for RobloxCS transpiler.");
}
