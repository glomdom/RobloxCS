#nullable enable
namespace RobloxCS.Types;
[RobloxNative("PlayerData", RobloxNativeType.Instance)]
public partial class PlayerData : Instance  {
    public Player GetPlayer() => throw new InvalidOperationException("Cannot call reserved method for RobloxCS transpiler.");
    public PlayerDataRecord GetRecordAsync() => throw new InvalidOperationException("Cannot call reserved method for RobloxCS transpiler.");
}
