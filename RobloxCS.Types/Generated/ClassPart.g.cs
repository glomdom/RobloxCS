#nullable enable
namespace RobloxCS.Types;
[RobloxNative("Part", RobloxNativeType.Instance)]
public partial class Part : FormFactorPart  {
    public Enums.PartType Shape { get; set; } = default!;
}
