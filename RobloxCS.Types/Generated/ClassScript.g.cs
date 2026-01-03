#nullable enable
namespace RobloxCS.Types;
[RobloxNative("Script", RobloxNativeType.Instance)]
public partial class Script : BaseScript  {
    public string Source { get; } = default!;
}
