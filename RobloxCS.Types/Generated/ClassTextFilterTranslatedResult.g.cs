#nullable enable
namespace RobloxCS.Types;
[RobloxNative("TextFilterTranslatedResult", RobloxNativeType.Instance)]
public partial class TextFilterTranslatedResult : Instance  {
    public string SourceLanguage { get; set; } = default!;
    public TextFilterResult SourceText { get; set; } = default!;
    public TextFilterResult GetTranslationForLocale() => throw new InvalidOperationException("Cannot call reserved method for RobloxCS transpiler.");
    public Dictionary<string, object> GetTranslations() => throw new InvalidOperationException("Cannot call reserved method for RobloxCS transpiler.");
}
