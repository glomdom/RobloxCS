namespace RobloxCS.Types;

[RobloxNative("RBXScriptConnection", RobloxNativeType.DataType)]
public class RBXScriptConnection {
    public bool Connected { get; init; }

    public void Disconnect() => throw new InvalidOperationException("Cannot call reserved method for RobloxCS transpiler.");
}