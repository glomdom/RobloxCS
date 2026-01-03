#nullable enable
namespace RobloxCS.Types;
[RobloxNative("Handles", RobloxNativeType.Instance)]
public partial class Handles : HandlesBase  {
    public Faces Faces { get; } = default!;
    public Enums.HandlesStyle Style { get; } = default!;
}
