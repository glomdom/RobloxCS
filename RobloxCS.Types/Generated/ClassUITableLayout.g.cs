#nullable enable
namespace RobloxCS.Types;
[RobloxNative("UITableLayout", RobloxNativeType.Instance)]
public partial class UITableLayout : UIGridStyleLayout  {
    public bool FillEmptySpaceColumns { get; } = default!;
    public bool FillEmptySpaceRows { get; } = default!;
    public Enums.TableMajorAxis MajorAxis { get; } = default!;
    public UDim2 Padding { get; } = default!;
}
