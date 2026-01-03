#nullable enable
namespace RobloxCS.Types;
[RobloxNative("Players", RobloxNativeType.Service)]
public static partial class PlayersService {
    public static bool BanningEnabled { get; } = default!;
    public static bool BubbleChat { get; } = default!;
    public static bool CharacterAutoLoads { get; } = default!;
    public static bool ClassicChat { get; } = default!;
    public static Player LocalPlayer { get; } = default!;
    public static int MaxPlayers { get; } = default!;
    public static int PreferredPlayers { get; } = default!;
    public static float RespawnTime { get; } = default!;
    public static bool UseStrafingAnimations { get; } = default!;
    public static Player GetPlayerByUserId() => throw new InvalidOperationException("Cannot call reserved method for RobloxCS transpiler.");
    public static Player GetPlayerFromCharacter() => throw new InvalidOperationException("Cannot call reserved method for RobloxCS transpiler.");
    public static List<Instance> GetPlayers() => throw new InvalidOperationException("Cannot call reserved method for RobloxCS transpiler.");
    public static void BanAsync() => throw new InvalidOperationException("Cannot call reserved method for RobloxCS transpiler.");
    public static Model CreateHumanoidModelFromDescriptionAsync() => throw new InvalidOperationException("Cannot call reserved method for RobloxCS transpiler.");
    public static Model CreateHumanoidModelFromUserIdAsync() => throw new InvalidOperationException("Cannot call reserved method for RobloxCS transpiler.");
    public static BanHistoryPages GetBanHistoryAsync() => throw new InvalidOperationException("Cannot call reserved method for RobloxCS transpiler.");
    public static Dictionary<string, object> GetCharacterAppearanceInfoAsync() => throw new InvalidOperationException("Cannot call reserved method for RobloxCS transpiler.");
    public static FriendPages GetFriendsAsync() => throw new InvalidOperationException("Cannot call reserved method for RobloxCS transpiler.");
    public static HumanoidDescription GetHumanoidDescriptionFromOutfitIdAsync() => throw new InvalidOperationException("Cannot call reserved method for RobloxCS transpiler.");
    public static HumanoidDescription GetHumanoidDescriptionFromUserIdAsync() => throw new InvalidOperationException("Cannot call reserved method for RobloxCS transpiler.");
    public static string GetNameFromUserIdAsync() => throw new InvalidOperationException("Cannot call reserved method for RobloxCS transpiler.");
    public static int GetUserIdFromNameAsync() => throw new InvalidOperationException("Cannot call reserved method for RobloxCS transpiler.");
    public static LuaTuple GetUserThumbnailAsync() => throw new InvalidOperationException("Cannot call reserved method for RobloxCS transpiler.");
    public static void UnbanAsync() => throw new InvalidOperationException("Cannot call reserved method for RobloxCS transpiler.");
}
