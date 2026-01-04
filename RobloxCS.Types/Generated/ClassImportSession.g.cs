#nullable enable
namespace RobloxCS.Types;
[RobloxNative("ImportSession", RobloxNativeType.Instance)]
public partial class ImportSession : Instance  {
    public RBXScriptSignal<Dictionary<string, object>> UploadComplete { get; private set; } = null!;
    public RBXScriptSignal<float> UploadProgress { get; private set; } = null!;
}
