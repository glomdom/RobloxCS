#nullable enable
namespace RobloxCS.Types;
[RobloxNative("UIGridLayout", RobloxNativeType.Instance)]
public partial class UIGridLayout : UIGridStyleLayout  {
    public Vector2 AbsoluteCellCount { get; } = default!;
    public Vector2 AbsoluteCellSize { get; } = default!;
    public UDim2 CellPadding { get; } = default!;
    public UDim2 CellSize { get; } = default!;
    public int FillDirectionMaxCells { get; } = default!;
    public Enums.StartCorner StartCorner { get; } = default!;
}
