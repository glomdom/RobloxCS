#nullable enable
namespace RobloxCS.Types;
[RobloxNative("Motor6D", RobloxNativeType.Instance)]
public partial class Motor6D : Motor  {
    public string ChildName { get; set; } = default!;
    public string ParentName { get; set; } = default!;
}
