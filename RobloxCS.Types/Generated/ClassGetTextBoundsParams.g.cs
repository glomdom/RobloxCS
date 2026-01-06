#nullable enable
namespace RobloxCS.Types;
[RobloxNative("GetTextBoundsParams", RobloxNativeType.Instance)]
public partial class GetTextBoundsParams : Instance  {
    public Enums.Font Font { get; set; } = default!;
    public bool RichText { get; set; } = default!;
    public float Size { get; set; } = default!;
    public string Text { get; set; } = default!;
    public float Width { get; set; } = default!;
}
