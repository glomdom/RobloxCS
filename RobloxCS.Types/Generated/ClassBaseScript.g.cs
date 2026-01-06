#nullable enable
namespace RobloxCS.Types;
[RobloxNative("BaseScript", RobloxNativeType.Instance)]
public partial class BaseScript : LuaSourceContainer  {
    public bool Disabled { get; set; } = default!;
    public bool Enabled { get; set; } = default!;
    public Enums.RunContext RunContext { get; } = default!;
}
