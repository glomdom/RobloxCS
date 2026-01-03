namespace RobloxCS.Types;
public static partial class Enums {
    public enum PathStatus {
        Success = 0,
        NoPath = 5,
        ClosestNoPath = 1,
        ClosestOutOfRange = 2,
        FailStartNotEmpty = 3,
        FailFinishNotEmpty = 4,
    }
}
