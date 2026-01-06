#nullable enable
namespace RobloxCS.Types;
[RobloxNative("Tool", RobloxNativeType.Instance)]
public partial class Tool : BackpackItem  {
    public bool CanBeDropped { get; set; } = default!;
    public bool Enabled { get; set; } = default!;
    public CFrame Grip { get; set; } = default!;
    public bool ManualActivationOnly { get; set; } = default!;
    public bool RequiresHandle { get; set; } = default!;
    public string ToolTip { get; set; } = default!;
    public void Activate() => throw new InvalidOperationException("Cannot call reserved method for RobloxCS transpiler.");
    public void Deactivate() => throw new InvalidOperationException("Cannot call reserved method for RobloxCS transpiler.");
    public RBXScriptSignal Activated { get; private set; } = null!;
    public RBXScriptSignal Deactivated { get; private set; } = null!;
    public RBXScriptSignal<Mouse> Equipped { get; private set; } = null!;
    public RBXScriptSignal Unequipped { get; private set; } = null!;
}
