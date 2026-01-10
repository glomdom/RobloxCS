#nullable enable
namespace RobloxCS.Types;
[RobloxNative("PlayerDataService", RobloxNativeType.Service)]
public static partial class PlayerDataService {
    public static Enums.PlayerDataLoadFailureBehavior LoadFailureBehavior { get; set; } = default!;
    public static PlayerDataRecordConfig GetRecordConfig() => ThrowHelper.ThrowTranspiledMethod();
}
