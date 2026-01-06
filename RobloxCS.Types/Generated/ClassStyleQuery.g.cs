#nullable enable
namespace RobloxCS.Types;
[RobloxNative("StyleQuery", RobloxNativeType.Instance)]
public partial class StyleQuery : Instance  {
    public NumberRange AspectRatioRange { get; set; } = default!;
    public bool IsActive { get; set; } = default!;
    public Vector2 MaxSize { get; set; } = default!;
    public Vector2 MinSize { get; set; } = default!;
    public object GetCondition() => throw new InvalidOperationException("Cannot call reserved method for RobloxCS transpiler.");
    public Dictionary<string, object> GetConditions() => throw new InvalidOperationException("Cannot call reserved method for RobloxCS transpiler.");
    public void SetCondition() => throw new InvalidOperationException("Cannot call reserved method for RobloxCS transpiler.");
    public void SetConditions() => throw new InvalidOperationException("Cannot call reserved method for RobloxCS transpiler.");
}
