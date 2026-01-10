namespace RobloxCS.Types;

public static class ThrowHelper {
    public static dynamic ThrowTranspiledMethod() => throw new InvalidOperationException("Cannot call reserved method for RobloxCS transpiler.");
}