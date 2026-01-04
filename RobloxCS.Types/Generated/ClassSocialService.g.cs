#nullable enable
namespace RobloxCS.Types;
[RobloxNative("SocialService", RobloxNativeType.Service)]
public static partial class SocialService {
    public static List<Instance> GetPlayersByPartyId() => throw new InvalidOperationException("Cannot call reserved method for RobloxCS transpiler.");
    public static void HideSelfView() => throw new InvalidOperationException("Cannot call reserved method for RobloxCS transpiler.");
    public static void PromptGameInvite() => throw new InvalidOperationException("Cannot call reserved method for RobloxCS transpiler.");
    public static void PromptPhoneBook() => throw new InvalidOperationException("Cannot call reserved method for RobloxCS transpiler.");
    public static void ShowSelfView() => throw new InvalidOperationException("Cannot call reserved method for RobloxCS transpiler.");
    public static bool CanSendCallInviteAsync() => throw new InvalidOperationException("Cannot call reserved method for RobloxCS transpiler.");
    public static bool CanSendGameInviteAsync() => throw new InvalidOperationException("Cannot call reserved method for RobloxCS transpiler.");
    public static Enums.RsvpStatus GetEventRsvpStatusAsync() => throw new InvalidOperationException("Cannot call reserved method for RobloxCS transpiler.");
    public static Dictionary<string, object>? GetExperienceEventAsync() => throw new InvalidOperationException("Cannot call reserved method for RobloxCS transpiler.");
    public static List<object> GetPartyAsync() => throw new InvalidOperationException("Cannot call reserved method for RobloxCS transpiler.");
    public static List<object> GetUpcomingExperienceEventsAsync() => throw new InvalidOperationException("Cannot call reserved method for RobloxCS transpiler.");
    public static void PromptFeedbackSubmissionAsync() => throw new InvalidOperationException("Cannot call reserved method for RobloxCS transpiler.");
    public static LuaTuple PromptLinkSharingAsync() => throw new InvalidOperationException("Cannot call reserved method for RobloxCS transpiler.");
    public static Enums.RsvpStatus PromptRsvpToEventAsync() => throw new InvalidOperationException("Cannot call reserved method for RobloxCS transpiler.");
    public static RBXScriptSignal<Instance, Enums.InviteState> CallInviteStateChanged { get; private set; } = null!;
    public static RBXScriptSignal<Instance, List<object>> GameInvitePromptClosed { get; private set; } = null!;
    public static RBXScriptSignal<Instance> PhoneBookPromptClosed { get; private set; } = null!;
    public static RBXScriptSignal<Player> ShareSheetClosed { get; private set; } = null!;
}
