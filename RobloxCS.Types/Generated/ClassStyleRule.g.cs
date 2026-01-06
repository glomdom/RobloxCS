#nullable enable
namespace RobloxCS.Types;
[RobloxNative("StyleRule", RobloxNativeType.Instance)]
public partial class StyleRule : StyleBase  {
    public int Priority { get; set; } = default!;
    public string Selector { get; set; } = default!;
    public string SelectorError { get; set; } = default!;
    public Dictionary<string, object> GetProperties() => throw new InvalidOperationException("Cannot call reserved method for RobloxCS transpiler.");
    public object GetProperty() => throw new InvalidOperationException("Cannot call reserved method for RobloxCS transpiler.");
    public void SetProperties() => throw new InvalidOperationException("Cannot call reserved method for RobloxCS transpiler.");
    public void SetProperty() => throw new InvalidOperationException("Cannot call reserved method for RobloxCS transpiler.");
}
