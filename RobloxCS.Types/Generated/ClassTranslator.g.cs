#nullable enable
namespace RobloxCS.Types;
[RobloxNative("Translator", RobloxNativeType.Instance)]
public partial class Translator : Instance  {
    public string LocaleId { get; set; } = default!;
    public string FormatByKey() => ThrowHelper.ThrowTranspiledMethod();
    public string Translate() => ThrowHelper.ThrowTranspiledMethod();
}
