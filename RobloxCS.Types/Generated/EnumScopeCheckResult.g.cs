namespace RobloxCS.Types;
public static partial class Enums {
    public enum ScopeCheckResult {
        ConsentAccepted = 0,
        InvalidScopes = 1,
        Timeout = 2,
        NoUserInput = 3,
        BackendError = 4,
        UnexpectedError = 5,
        InvalidArgument = 6,
        ConsentDenied = 7,
    }
}
