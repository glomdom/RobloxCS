#nullable enable
namespace RobloxCS.Types;
[RobloxNative("RootImportData", RobloxNativeType.Instance)]
public partial class RootImportData : BaseImportData  {
    public bool AddModelToInventory { get; } = default!;
    public bool Anchored { get; } = default!;
    public float AnimationIdForRestPose { get; } = default!;
    public string ExistingPackageId { get; } = default!;
    public Vector3 FileDimensions { get; } = default!;
    public bool ImportAsModelAsset { get; } = default!;
    public bool ImportAsPackage { get; } = default!;
    public bool InsertInWorkspace { get; } = default!;
    public bool InsertWithScenePosition { get; } = default!;
    public bool InvertNegativeFaces { get; } = default!;
    public bool KeepZeroInfluenceBones { get; } = default!;
    public bool MergeMeshes { get; } = default!;
    public float PolygonCount { get; } = default!;
    public int PreferredUploadId { get; } = default!;
    public Enums.RestPose RestPose { get; } = default!;
    public Enums.RigScale RigScale { get; } = default!;
    public Enums.RigType RigType { get; } = default!;
    public bool RigVisualization { get; } = default!;
    public Enums.MeshScaleUnit ScaleUnit { get; } = default!;
    public bool UseSceneOriginAsPivot { get; } = default!;
    public bool UsesCages { get; } = default!;
    public bool ValidateUgcBody { get; } = default!;
    public Enums.NormalId WorldForward { get; } = default!;
    public Enums.NormalId WorldUp { get; } = default!;
}
