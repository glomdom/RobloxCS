#nullable enable
namespace RobloxCS.Types;
[RobloxNative("ModuleScript", RobloxNativeType.Instance)]
public partial class ModuleScript : LuaSourceContainer  {
    public string Source { get; set; } = default!;
}
