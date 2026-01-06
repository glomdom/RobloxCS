#nullable enable
namespace RobloxCS.Types;
[RobloxNative("DialogChoice", RobloxNativeType.Instance)]
public partial class DialogChoice : Instance  {
    public bool GoodbyeChoiceActive { get; set; } = default!;
    public string GoodbyeDialog { get; set; } = default!;
    public string ResponseDialog { get; set; } = default!;
    public string UserDialog { get; set; } = default!;
}
