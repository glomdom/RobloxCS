#nullable enable
namespace RobloxCS.Types;
[RobloxNative("UITableLayout", RobloxNativeType.Instance)]
public partial class UITableLayout : UIGridStyleLayout  {
    public bool FillEmptySpaceColumns { get; set; } = default!;
    public bool FillEmptySpaceRows { get; set; } = default!;
    public Enums.TableMajorAxis MajorAxis { get; set; } = default!;
    public UDim2 Padding { get; set; } = default!;
}
