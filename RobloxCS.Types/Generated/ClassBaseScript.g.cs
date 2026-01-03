#nullable enable
namespace RobloxCS.Types;
[RobloxNative("BaseScript", RobloxNativeType.Instance)]
public partial class BaseScript : LuaSourceContainer  {
    public bool Disabled { get; } = default!;
    public bool Enabled { get; } = default!;
    public Enums.RunContext RunContext { get; } = default!;
}
