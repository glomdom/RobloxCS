#nullable enable
namespace RobloxCS.Types;
[RobloxNative("SyncScriptBuilder", RobloxNativeType.Instance)]
public partial class SyncScriptBuilder : ScriptBuilder  {
    public Enums.CompileTarget CompileTarget { get; set; } = default!;
    public bool CoverageInfo { get; set; } = default!;
    public bool DebugInfo { get; set; } = default!;
    public bool PackAsSource { get; set; } = default!;
}
