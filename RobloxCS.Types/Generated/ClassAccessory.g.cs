#nullable enable
namespace RobloxCS.Types;
[RobloxNative("Accessory", RobloxNativeType.Instance)]
public partial class Accessory : Accoutrement  {
    public Enums.AccessoryType AccessoryType { get; } = default!;
}
