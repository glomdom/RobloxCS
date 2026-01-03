#nullable enable
namespace RobloxCS.Types;
[RobloxNative("Actor", RobloxNativeType.Instance)]
public partial class Actor : Model  {
    public RBXScriptConnection BindToMessage() => throw new InvalidOperationException("Cannot call reserved method for RobloxCS transpiler.");
    public RBXScriptConnection BindToMessageParallel() => throw new InvalidOperationException("Cannot call reserved method for RobloxCS transpiler.");
    public void SendMessage() => throw new InvalidOperationException("Cannot call reserved method for RobloxCS transpiler.");
}
