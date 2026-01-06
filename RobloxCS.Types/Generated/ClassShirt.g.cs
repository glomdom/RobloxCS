#nullable enable
namespace RobloxCS.Types;
[RobloxNative("Shirt", RobloxNativeType.Instance)]
public partial class Shirt : Clothing  {
    public string ShirtTemplate { get; set; } = default!;
}
