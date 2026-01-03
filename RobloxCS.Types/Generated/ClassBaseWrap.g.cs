#nullable enable
namespace RobloxCS.Types;
[RobloxNative("BaseWrap", RobloxNativeType.Instance)]
public partial class BaseWrap : Instance  {
    public string CageMeshContent { get; } = default!;
    public string CageMeshId { get; } = default!;
    public CFrame CageOrigin { get; } = default!;
    public CFrame CageOriginWorld { get; } = default!;
    public CFrame ImportOrigin { get; } = default!;
    public CFrame ImportOriginWorld { get; } = default!;
}
