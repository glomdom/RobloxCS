#nullable enable
namespace RobloxCS.Types;
[RobloxNative("RootImportData", RobloxNativeType.Instance)]
public partial class RootImportData : BaseImportData  {
    public bool AddModelToInventory { get; set; } = default!;
    public bool Anchored { get; set; } = default!;
    public float AnimationIdForRestPose { get; set; } = default!;
    public string ExistingPackageId { get; set; } = default!;
    public Vector3 FileDimensions { get; set; } = default!;
    public bool ImportAsModelAsset { get; set; } = default!;
    public bool ImportAsPackage { get; set; } = default!;
    public bool InsertInWorkspace { get; set; } = default!;
    public bool InsertWithScenePosition { get; set; } = default!;
    public bool InvertNegativeFaces { get; set; } = default!;
    public bool KeepZeroInfluenceBones { get; set; } = default!;
    public bool MergeMeshes { get; set; } = default!;
    public float PolygonCount { get; set; } = default!;
    public int PreferredUploadId { get; set; } = default!;
    public Enums.RestPose RestPose { get; set; } = default!;
    public Enums.RigScale RigScale { get; set; } = default!;
    public Enums.RigType RigType { get; set; } = default!;
    public bool RigVisualization { get; set; } = default!;
    public Enums.MeshScaleUnit ScaleUnit { get; set; } = default!;
    public bool UseSceneOriginAsPivot { get; set; } = default!;
    public bool UsesCages { get; set; } = default!;
    public bool ValidateUgcBody { get; set; } = default!;
    public Enums.NormalId WorldForward { get; set; } = default!;
    public Enums.NormalId WorldUp { get; set; } = default!;
}
