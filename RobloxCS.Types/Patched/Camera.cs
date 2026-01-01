namespace RobloxCS.Types;

public partial class Camera {
    public (Vector3, bool inBounds) WorldToScreenPoint() => throw new InvalidOperationException("Cannot call reserved method for RobloxCS transpiler.");
    public (Vector3, bool inBounds) WorldToViewportPoint() => throw new InvalidOperationException("Cannot call reserved method for RobloxCS transpiler.");
}