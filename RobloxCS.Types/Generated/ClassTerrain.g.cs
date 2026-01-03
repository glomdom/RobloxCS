#nullable enable
namespace RobloxCS.Types;
[RobloxNative("Terrain", RobloxNativeType.Instance)]
public partial class Terrain : BasePart  {
    public bool Decoration { get; } = default!;
    public float GrassLength { get; } = default!;
    public string MaterialColors { get; } = default!;
    public Region3int16 MaxExtents { get; } = default!;
    public Color3 WaterColor { get; } = default!;
    public float WaterReflectance { get; } = default!;
    public float WaterTransparency { get; } = default!;
    public float WaterWaveSize { get; } = default!;
    public float WaterWaveSpeed { get; } = default!;
    public Vector3 CellCenterToWorld() => throw new InvalidOperationException("Cannot call reserved method for RobloxCS transpiler.");
    public Vector3 CellCornerToWorld() => throw new InvalidOperationException("Cannot call reserved method for RobloxCS transpiler.");
    public void Clear() => throw new InvalidOperationException("Cannot call reserved method for RobloxCS transpiler.");
    public void ClearVoxelsAsync_beta() => throw new InvalidOperationException("Cannot call reserved method for RobloxCS transpiler.");
    public TerrainRegion CopyRegion() => throw new InvalidOperationException("Cannot call reserved method for RobloxCS transpiler.");
    public int CountCells() => throw new InvalidOperationException("Cannot call reserved method for RobloxCS transpiler.");
    public void FillBall() => throw new InvalidOperationException("Cannot call reserved method for RobloxCS transpiler.");
    public void FillBlock() => throw new InvalidOperationException("Cannot call reserved method for RobloxCS transpiler.");
    public void FillCylinder() => throw new InvalidOperationException("Cannot call reserved method for RobloxCS transpiler.");
    public void FillRegion() => throw new InvalidOperationException("Cannot call reserved method for RobloxCS transpiler.");
    public void FillWedge() => throw new InvalidOperationException("Cannot call reserved method for RobloxCS transpiler.");
    public Color3 GetMaterialColor() => throw new InvalidOperationException("Cannot call reserved method for RobloxCS transpiler.");
    public TerrainIterateOperation IterateVoxelsAsync_beta() => throw new InvalidOperationException("Cannot call reserved method for RobloxCS transpiler.");
    public TerrainModifyOperation ModifyVoxelsAsync_beta() => throw new InvalidOperationException("Cannot call reserved method for RobloxCS transpiler.");
    public void PasteRegion() => throw new InvalidOperationException("Cannot call reserved method for RobloxCS transpiler.");
    public Dictionary<string, object> ReadVoxelChannels() => throw new InvalidOperationException("Cannot call reserved method for RobloxCS transpiler.");
    public LuaTuple ReadVoxels() => throw new InvalidOperationException("Cannot call reserved method for RobloxCS transpiler.");
    public TerrainReadOperation ReadVoxelsAsync_beta() => throw new InvalidOperationException("Cannot call reserved method for RobloxCS transpiler.");
    public void ReplaceMaterial() => throw new InvalidOperationException("Cannot call reserved method for RobloxCS transpiler.");
    public void SetMaterialColor() => throw new InvalidOperationException("Cannot call reserved method for RobloxCS transpiler.");
    public Vector3 WorldToCell() => throw new InvalidOperationException("Cannot call reserved method for RobloxCS transpiler.");
    public Vector3 WorldToCellPreferEmpty() => throw new InvalidOperationException("Cannot call reserved method for RobloxCS transpiler.");
    public Vector3 WorldToCellPreferSolid() => throw new InvalidOperationException("Cannot call reserved method for RobloxCS transpiler.");
    public void WriteVoxelChannels() => throw new InvalidOperationException("Cannot call reserved method for RobloxCS transpiler.");
    public void WriteVoxels() => throw new InvalidOperationException("Cannot call reserved method for RobloxCS transpiler.");
    public TerrainWriteOperation WriteVoxelsAsync_beta() => throw new InvalidOperationException("Cannot call reserved method for RobloxCS transpiler.");
}
