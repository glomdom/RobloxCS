#nullable enable
namespace RobloxCS.Types;
[RobloxNative("Pages", RobloxNativeType.Instance)]
public partial class Pages : Instance  {
    public bool IsFinished { get; } = default!;
    public List<object> GetCurrentPage() => throw new InvalidOperationException("Cannot call reserved method for RobloxCS transpiler.");
    public void AdvanceToNextPageAsync() => throw new InvalidOperationException("Cannot call reserved method for RobloxCS transpiler.");
}
