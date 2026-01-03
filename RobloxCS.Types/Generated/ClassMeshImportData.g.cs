#nullable enable
namespace RobloxCS.Types;
[RobloxNative("MeshImportData", RobloxNativeType.Instance)]
public partial class MeshImportData : BaseImportData  {
    public bool Anchored { get; } = default!;
    public bool CageManifold { get; } = default!;
    public bool CageMeshIntersectedPreview { get; } = default!;
    public bool CageMeshNotIntersected { get; } = default!;
    public bool CageNoOverlappingVertices { get; } = default!;
    public bool CageNonManifoldPreview { get; } = default!;
    public bool CageOverlappingVerticesPreview { get; } = default!;
    public bool CageUVMatched { get; } = default!;
    public bool CageUVMisMatchedPreview { get; } = default!;
    public Vector3 Dimensions { get; } = default!;
    public bool DoubleSided { get; } = default!;
    public bool IgnoreVertexColors { get; } = default!;
    public bool IrrelevantCageModifiedPreview { get; } = default!;
    public bool MeshHoleDetectedPreview { get; } = default!;
    public bool MeshNoHoleDetected { get; } = default!;
    public bool NoIrrelevantCageModified { get; } = default!;
    public bool NoOuterCageFarExtendedFromMesh { get; } = default!;
    public bool OuterCageFarExtendedFromMeshPreview { get; } = default!;
    public float PolygonCount { get; } = default!;
    public bool UseImportedPivot { get; } = default!;
}
