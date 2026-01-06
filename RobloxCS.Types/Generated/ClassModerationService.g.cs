#nullable enable
namespace RobloxCS.Types;
[RobloxNative("ModerationService", RobloxNativeType.Service)]
public static partial class ModerationService {
    public static RBXScriptConnection BindReviewableContentEventProcessor() => throw new InvalidOperationException("Cannot call reserved method for RobloxCS transpiler.");
    public static string CreateReviewableContentKey() => throw new InvalidOperationException("Cannot call reserved method for RobloxCS transpiler.");
    public static string CreateReviewableContentAsync() => throw new InvalidOperationException("Cannot call reserved method for RobloxCS transpiler.");
    public static string InternalCreateReviewableContentAsync() => throw new InvalidOperationException("Cannot call reserved method for RobloxCS transpiler.");
    public static void InternalRequestReviewableContentReviewAsync() => throw new InvalidOperationException("Cannot call reserved method for RobloxCS transpiler.");
    public static Func<Dictionary<string, object>> InternalProcessReviewableContentEvent { get; set; } = default!;
}
