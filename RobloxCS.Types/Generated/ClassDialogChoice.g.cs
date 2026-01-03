#nullable enable
namespace RobloxCS.Types;
[RobloxNative("DialogChoice", RobloxNativeType.Instance)]
public partial class DialogChoice : Instance  {
    public bool GoodbyeChoiceActive { get; } = default!;
    public string GoodbyeDialog { get; } = default!;
    public string ResponseDialog { get; } = default!;
    public string UserDialog { get; } = default!;
}
