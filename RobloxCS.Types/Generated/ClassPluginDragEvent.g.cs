#nullable enable
namespace RobloxCS.Types;
[RobloxNative("PluginDragEvent", RobloxNativeType.Instance)]
public partial class PluginDragEvent : Instance  {
    public string Data { get; set; } = default!;
    public string MimeType { get; set; } = default!;
    public Vector2 Position { get; set; } = default!;
    public string Sender { get; set; } = default!;
}
