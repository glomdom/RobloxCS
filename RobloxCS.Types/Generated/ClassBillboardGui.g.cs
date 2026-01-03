#nullable enable
namespace RobloxCS.Types;
[RobloxNative("BillboardGui", RobloxNativeType.Instance)]
public partial class BillboardGui : LayerCollector  {
    public bool Active { get; } = default!;
    public Instance Adornee { get; } = default!;
    public bool AlwaysOnTop { get; } = default!;
    public float Brightness { get; } = default!;
    public bool ClipsDescendants { get; } = default!;
    public float CurrentDistance { get; } = default!;
    public float DistanceStep { get; } = default!;
    public Vector3 ExtentsOffset { get; } = default!;
    public Vector3 ExtentsOffsetWorldSpace { get; } = default!;
    public float LightInfluence { get; } = default!;
    public float MaxDistance { get; } = default!;
    public Instance PlayerToHideFrom { get; } = default!;
    public UDim2 Size { get; } = default!;
    public Vector2 SizeOffset { get; } = default!;
    public Vector3 StudsOffset { get; } = default!;
    public Vector3 StudsOffsetWorldSpace { get; } = default!;
}
