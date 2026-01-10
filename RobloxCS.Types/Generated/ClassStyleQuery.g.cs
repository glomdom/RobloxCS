#nullable enable
namespace RobloxCS.Types;
[RobloxNative("StyleQuery", RobloxNativeType.Instance)]
public partial class StyleQuery : Instance  {
    public NumberRange AspectRatioRange { get; set; } = default!;
    public bool IsActive { get; set; } = default!;
    public Vector2 MaxSize { get; set; } = default!;
    public Vector2 MinSize { get; set; } = default!;
    public object GetCondition() => ThrowHelper.ThrowTranspiledMethod();
    public Dictionary<string, object> GetConditions() => ThrowHelper.ThrowTranspiledMethod();
    public void SetCondition() => ThrowHelper.ThrowTranspiledMethod();
    public void SetConditions() => ThrowHelper.ThrowTranspiledMethod();
}
