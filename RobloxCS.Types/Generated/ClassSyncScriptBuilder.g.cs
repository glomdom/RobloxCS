#nullable enable
namespace RobloxCS.Types;
[RobloxNative("SyncScriptBuilder", RobloxNativeType.Instance)]
public partial class SyncScriptBuilder : ScriptBuilder  {
    public Enums.CompileTarget CompileTarget { get; } = default!;
    public bool CoverageInfo { get; } = default!;
    public bool DebugInfo { get; } = default!;
    public bool PackAsSource { get; } = default!;
}
