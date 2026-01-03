#nullable enable
namespace RobloxCS.Types;
[RobloxNative("DebuggerWatch", RobloxNativeType.Instance)]
public partial class DebuggerWatch : Instance  {
    public string Expression { get; } = default!;
}
