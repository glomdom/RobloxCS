#nullable enable
namespace RobloxCS.Types;
[RobloxNative("Tool", RobloxNativeType.Instance)]
public partial class Tool : BackpackItem  {
    public bool CanBeDropped { get; } = default!;
    public bool Enabled { get; } = default!;
    public CFrame Grip { get; } = default!;
    public bool ManualActivationOnly { get; } = default!;
    public bool RequiresHandle { get; } = default!;
    public string ToolTip { get; } = default!;
    public void Activate() => throw new InvalidOperationException("Cannot call reserved method for RobloxCS transpiler.");
    public void Deactivate() => throw new InvalidOperationException("Cannot call reserved method for RobloxCS transpiler.");
}
