#nullable enable
namespace RobloxCS.Types;
[RobloxNative("PluginDragEvent", RobloxNativeType.Instance)]
public partial class PluginDragEvent : Instance  {
    public string Data { get; } = default!;
    public string MimeType { get; } = default!;
    public Vector2 Position { get; } = default!;
    public string Sender { get; } = default!;
}
