#nullable enable
namespace RobloxCS.Types;
[RobloxNative("InputObject", RobloxNativeType.Instance)]
public partial class InputObject : Instance  {
    public Vector3 Delta { get; set; } = default!;
    public Enums.KeyCode KeyCode { get; set; } = default!;
    public Vector3 Position { get; set; } = default!;
    public Enums.UserInputState UserInputState { get; set; } = default!;
    public Enums.UserInputType UserInputType { get; set; } = default!;
    public bool IsModifierKeyDown() => throw new InvalidOperationException("Cannot call reserved method for RobloxCS transpiler.");
}
