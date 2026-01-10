#nullable enable
namespace RobloxCS.Types;
[RobloxNative("Pages", RobloxNativeType.Instance)]
public partial class Pages : Instance  {
    public bool IsFinished { get; set; } = default!;
    public List<object> GetCurrentPage() => ThrowHelper.ThrowTranspiledMethod();
    public void AdvanceToNextPageAsync() => ThrowHelper.ThrowTranspiledMethod();
}
