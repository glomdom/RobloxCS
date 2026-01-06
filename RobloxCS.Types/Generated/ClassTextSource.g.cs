#nullable enable
namespace RobloxCS.Types;
[RobloxNative("TextSource", RobloxNativeType.Instance)]
public partial class TextSource : Instance  {
    public bool CanSend { get; set; } = default!;
    public int UserId { get; set; } = default!;
}
