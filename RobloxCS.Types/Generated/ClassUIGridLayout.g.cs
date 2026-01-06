#nullable enable
namespace RobloxCS.Types;
[RobloxNative("UIGridLayout", RobloxNativeType.Instance)]
public partial class UIGridLayout : UIGridStyleLayout  {
    public Vector2 AbsoluteCellCount { get; set; } = default!;
    public Vector2 AbsoluteCellSize { get; set; } = default!;
    public UDim2 CellPadding { get; set; } = default!;
    public UDim2 CellSize { get; set; } = default!;
    public int FillDirectionMaxCells { get; set; } = default!;
    public Enums.StartCorner StartCorner { get; set; } = default!;
}
