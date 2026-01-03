#nullable enable
namespace RobloxCS.Types;
[RobloxNative("MaterialImportData", RobloxNativeType.Instance)]
public partial class MaterialImportData : BaseImportData  {
    public string DiffuseFilePath { get; } = default!;
    public string EmissiveFilePath { get; } = default!;
    public bool IsPbr { get; } = default!;
    public string MetalnessFilePath { get; } = default!;
    public string NormalFilePath { get; } = default!;
    public string RoughnessFilePath { get; } = default!;
}
