#nullable enable
namespace RobloxCS.Types;
[RobloxNative("BindableFunction", RobloxNativeType.Instance)]
public partial class BindableFunction : Instance  {
    public LuaTuple Invoke() => ThrowHelper.ThrowTranspiledMethod();
    public Func<LuaTuple> OnInvoke { get; set; } = default!;
}
