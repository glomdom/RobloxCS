#nullable enable
namespace RobloxCS.Types;
[RobloxNative("InputObject", RobloxNativeType.Instance)]
public partial class InputObject : Instance  {
    public Vector3 Delta { get; } = default!;
    public Enums.KeyCode KeyCode { get; } = default!;
    public Vector3 Position { get; } = default!;
    public Enums.UserInputState UserInputState { get; } = default!;
    public Enums.UserInputType UserInputType { get; } = default!;
    public bool IsModifierKeyDown() => throw new InvalidOperationException("Cannot call reserved method for RobloxCS transpiler.");
}
