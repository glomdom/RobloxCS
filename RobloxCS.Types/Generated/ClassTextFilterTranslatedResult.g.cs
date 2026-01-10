#nullable enable
namespace RobloxCS.Types;
[RobloxNative("TextFilterTranslatedResult", RobloxNativeType.Instance)]
public partial class TextFilterTranslatedResult : Instance  {
    public string SourceLanguage { get; set; } = default!;
    public TextFilterResult SourceText { get; set; } = default!;
    public TextFilterResult GetTranslationForLocale() => ThrowHelper.ThrowTranspiledMethod();
    public Dictionary<string, object> GetTranslations() => ThrowHelper.ThrowTranspiledMethod();
}
