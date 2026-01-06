#nullable enable
namespace RobloxCS.Types;
[RobloxNative("LocalizationTable", RobloxNativeType.Instance)]
public partial class LocalizationTable : Instance  {
    public string SourceLocaleId { get; set; } = default!;
    public List<object> GetEntries() => throw new InvalidOperationException("Cannot call reserved method for RobloxCS transpiler.");
    public Instance GetTranslator() => throw new InvalidOperationException("Cannot call reserved method for RobloxCS transpiler.");
    public void RemoveEntry() => throw new InvalidOperationException("Cannot call reserved method for RobloxCS transpiler.");
    public void RemoveEntryValue() => throw new InvalidOperationException("Cannot call reserved method for RobloxCS transpiler.");
    public void RemoveTargetLocale() => throw new InvalidOperationException("Cannot call reserved method for RobloxCS transpiler.");
    public void SetEntries() => throw new InvalidOperationException("Cannot call reserved method for RobloxCS transpiler.");
    public void SetEntryContext() => throw new InvalidOperationException("Cannot call reserved method for RobloxCS transpiler.");
    public void SetEntryExample() => throw new InvalidOperationException("Cannot call reserved method for RobloxCS transpiler.");
    public void SetEntryKey() => throw new InvalidOperationException("Cannot call reserved method for RobloxCS transpiler.");
    public void SetEntrySource() => throw new InvalidOperationException("Cannot call reserved method for RobloxCS transpiler.");
    public void SetEntryValue() => throw new InvalidOperationException("Cannot call reserved method for RobloxCS transpiler.");
}
