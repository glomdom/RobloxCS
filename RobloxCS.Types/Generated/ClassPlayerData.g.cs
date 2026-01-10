#nullable enable
namespace RobloxCS.Types;
[RobloxNative("PlayerData", RobloxNativeType.Instance)]
public partial class PlayerData : Instance  {
    public Player GetPlayer() => ThrowHelper.ThrowTranspiledMethod();
    public PlayerDataRecord GetRecordAsync() => ThrowHelper.ThrowTranspiledMethod();
}
