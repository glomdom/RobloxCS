#nullable enable
namespace RobloxCS.Types;
[RobloxNative("StyleRule", RobloxNativeType.Instance)]
public partial class StyleRule : StyleBase  {
    public int Priority { get; set; } = default!;
    public string Selector { get; set; } = default!;
    public string SelectorError { get; set; } = default!;
    public Dictionary<string, object> GetProperties() => ThrowHelper.ThrowTranspiledMethod();
    public object GetProperty() => ThrowHelper.ThrowTranspiledMethod();
    public void SetProperties() => ThrowHelper.ThrowTranspiledMethod();
    public void SetProperty() => ThrowHelper.ThrowTranspiledMethod();
}
