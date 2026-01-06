#nullable enable
namespace RobloxCS.Types;
[RobloxNative("LocalizationService", RobloxNativeType.Service)]
public static partial class LocalizationService {
    public static string RobloxLocaleId { get; set; } = default!;
    public static string SystemLocaleId { get; set; } = default!;
    public static List<Instance> GetCorescriptLocalizations() => throw new InvalidOperationException("Cannot call reserved method for RobloxCS transpiler.");
    public static List<object> GetTableEntries() => throw new InvalidOperationException("Cannot call reserved method for RobloxCS transpiler.");
    public static Instance GetTranslatorForPlayer() => throw new InvalidOperationException("Cannot call reserved method for RobloxCS transpiler.");
    public static string GetCountryRegionForPlayerAsync() => throw new InvalidOperationException("Cannot call reserved method for RobloxCS transpiler.");
    public static Instance GetTranslatorForLocaleAsync() => throw new InvalidOperationException("Cannot call reserved method for RobloxCS transpiler.");
    public static Instance GetTranslatorForPlayerAsync() => throw new InvalidOperationException("Cannot call reserved method for RobloxCS transpiler.");
}
