#nullable enable
namespace RobloxCS.Types;
[RobloxNative("GetTextBoundsParams", RobloxNativeType.Instance)]
public partial class GetTextBoundsParams : Instance  {
    public Enums.Font Font { get; } = default!;
    public bool RichText { get; } = default!;
    public float Size { get; } = default!;
    public string Text { get; } = default!;
    public float Width { get; } = default!;
}
