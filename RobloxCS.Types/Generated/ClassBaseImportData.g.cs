#nullable enable
namespace RobloxCS.Types;
[RobloxNative("BaseImportData", RobloxNativeType.Instance)]
public partial class BaseImportData : Instance  {
    public string Id { get; } = default!;
    public string ImportName { get; } = default!;
    public bool ShouldImport { get; } = default!;
    public RBXScriptSignal<Dictionary<string, object>> StatusRemoved { get; private set; } = null!;
    public RBXScriptSignal<Dictionary<string, object>> StatusReported { get; private set; } = null!;
}
