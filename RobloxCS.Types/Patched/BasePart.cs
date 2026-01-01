namespace RobloxCS.Types;

public partial class BasePart {
    public (bool canSet, string cannotSetReason) CanSetNetworkOwnership() => throw new InvalidOperationException("Cannot call reserved method for RobloxCS transpiler.");
}