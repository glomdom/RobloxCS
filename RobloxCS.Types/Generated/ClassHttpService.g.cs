#nullable enable
namespace RobloxCS.Types;
[RobloxNative("HttpService", RobloxNativeType.Service)]
public static partial class HttpService {
    public static bool HttpEnabled { get; } = default!;
    public static WebStreamClient CreateWebStreamClient() => throw new InvalidOperationException("Cannot call reserved method for RobloxCS transpiler.");
    public static string GenerateGUID() => throw new InvalidOperationException("Cannot call reserved method for RobloxCS transpiler.");
    public static Secret GetSecret() => throw new InvalidOperationException("Cannot call reserved method for RobloxCS transpiler.");
    public static object JSONDecode() => throw new InvalidOperationException("Cannot call reserved method for RobloxCS transpiler.");
    public static string JSONEncode() => throw new InvalidOperationException("Cannot call reserved method for RobloxCS transpiler.");
    public static string UrlEncode() => throw new InvalidOperationException("Cannot call reserved method for RobloxCS transpiler.");
    public static string GetAsync() => throw new InvalidOperationException("Cannot call reserved method for RobloxCS transpiler.");
    public static string PostAsync() => throw new InvalidOperationException("Cannot call reserved method for RobloxCS transpiler.");
    public static Dictionary<string, object> RequestAsync() => throw new InvalidOperationException("Cannot call reserved method for RobloxCS transpiler.");
}
