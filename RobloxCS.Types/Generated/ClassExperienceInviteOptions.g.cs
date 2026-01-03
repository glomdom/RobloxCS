#nullable enable
namespace RobloxCS.Types;
[RobloxNative("ExperienceInviteOptions", RobloxNativeType.Instance)]
public partial class ExperienceInviteOptions : Instance  {
    public string InviteMessageId { get; } = default!;
    public int InviteUser { get; } = default!;
    public string LaunchData { get; } = default!;
    public string PromptMessage { get; } = default!;
}
