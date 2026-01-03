#nullable enable
namespace RobloxCS.Types;
[RobloxNative("CFrameValue", RobloxNativeType.Instance)]
public partial class CFrameValue : ValueBase  {
    public CFrame Value { get; } = default!;
}
