#nullable enable
namespace RobloxCS.Types;
[RobloxNative("Translator", RobloxNativeType.Instance)]
public partial class Translator : Instance  {
    public string LocaleId { get; set; } = default!;
    public string FormatByKey() => throw new InvalidOperationException("Cannot call reserved method for RobloxCS transpiler.");
    public string Translate() => throw new InvalidOperationException("Cannot call reserved method for RobloxCS transpiler.");
}
