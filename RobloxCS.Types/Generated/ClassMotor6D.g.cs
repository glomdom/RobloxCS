#nullable enable
namespace RobloxCS.Types;
[RobloxNative("Motor6D", RobloxNativeType.Instance)]
public partial class Motor6D : Motor  {
    public string ChildName { get; } = default!;
    public string ParentName { get; } = default!;
}
