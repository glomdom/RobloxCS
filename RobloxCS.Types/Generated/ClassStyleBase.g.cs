#nullable enable
namespace RobloxCS.Types;
[RobloxNative("StyleBase", RobloxNativeType.Instance)]
public partial class StyleBase : Instance  {
    public List<Instance> GetStyleRules() => ThrowHelper.ThrowTranspiledMethod();
    public void InsertStyleRule() => ThrowHelper.ThrowTranspiledMethod();
    public void SetStyleRules() => ThrowHelper.ThrowTranspiledMethod();
    public RBXScriptSignal StyleRulesChanged { get; private set; } = null!;
}
