#nullable enable
namespace RobloxCS.Types;
[RobloxNative("Terrain", RobloxNativeType.Instance)]
public partial class Terrain : BasePart  {
    public bool Decoration { get; set; } = default!;
    public float GrassLength { get; set; } = default!;
    public string MaterialColors { get; set; } = default!;
    public Region3int16 MaxExtents { get; set; } = default!;
    public Color3 WaterColor { get; set; } = default!;
    public float WaterReflectance { get; set; } = default!;
    public float WaterTransparency { get; set; } = default!;
    public float WaterWaveSize { get; set; } = default!;
    public float WaterWaveSpeed { get; set; } = default!;
    public Vector3 CellCenterToWorld() => ThrowHelper.ThrowTranspiledMethod();
    public Vector3 CellCornerToWorld() => ThrowHelper.ThrowTranspiledMethod();
    public void Clear() => ThrowHelper.ThrowTranspiledMethod();
    public void ClearVoxelsAsync_beta() => ThrowHelper.ThrowTranspiledMethod();
    public TerrainRegion CopyRegion() => ThrowHelper.ThrowTranspiledMethod();
    public int CountCells() => ThrowHelper.ThrowTranspiledMethod();
    public void FillBall() => ThrowHelper.ThrowTranspiledMethod();
    public void FillBlock() => ThrowHelper.ThrowTranspiledMethod();
    public void FillCylinder() => ThrowHelper.ThrowTranspiledMethod();
    public void FillRegion() => ThrowHelper.ThrowTranspiledMethod();
    public void FillWedge() => ThrowHelper.ThrowTranspiledMethod();
    public Color3 GetMaterialColor() => ThrowHelper.ThrowTranspiledMethod();
    public TerrainIterateOperation IterateVoxelsAsync_beta() => ThrowHelper.ThrowTranspiledMethod();
    public TerrainModifyOperation ModifyVoxelsAsync_beta() => ThrowHelper.ThrowTranspiledMethod();
    public void PasteRegion() => ThrowHelper.ThrowTranspiledMethod();
    public Dictionary<string, object> ReadVoxelChannels() => ThrowHelper.ThrowTranspiledMethod();
    public LuaTuple ReadVoxels() => ThrowHelper.ThrowTranspiledMethod();
    public TerrainReadOperation ReadVoxelsAsync_beta() => ThrowHelper.ThrowTranspiledMethod();
    public void ReplaceMaterial() => ThrowHelper.ThrowTranspiledMethod();
    public void SetMaterialColor() => ThrowHelper.ThrowTranspiledMethod();
    public Vector3 WorldToCell() => ThrowHelper.ThrowTranspiledMethod();
    public Vector3 WorldToCellPreferEmpty() => ThrowHelper.ThrowTranspiledMethod();
    public Vector3 WorldToCellPreferSolid() => ThrowHelper.ThrowTranspiledMethod();
    public void WriteVoxelChannels() => ThrowHelper.ThrowTranspiledMethod();
    public void WriteVoxels() => ThrowHelper.ThrowTranspiledMethod();
    public TerrainWriteOperation WriteVoxelsAsync_beta() => ThrowHelper.ThrowTranspiledMethod();
}
