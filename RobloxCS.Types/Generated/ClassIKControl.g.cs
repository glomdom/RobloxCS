#nullable enable
namespace RobloxCS.Types;
[RobloxNative("IKControl", RobloxNativeType.Instance)]
public partial class IKControl : Instance  {
    public Instance ChainRoot { get; set; } = default!;
    public bool Enabled { get; set; } = default!;
    public Instance EndEffector { get; set; } = default!;
    public CFrame EndEffectorOffset { get; set; } = default!;
    public CFrame Offset { get; set; } = default!;
    public Instance Pole { get; set; } = default!;
    public int Priority { get; set; } = default!;
    public float SmoothTime { get; set; } = default!;
    public Instance Target { get; set; } = default!;
    public Enums.IKControlType Type { get; set; } = default!;
    public float Weight { get; set; } = default!;
    public int GetChainCount() => throw new InvalidOperationException("Cannot call reserved method for RobloxCS transpiler.");
    public float GetChainLength() => throw new InvalidOperationException("Cannot call reserved method for RobloxCS transpiler.");
    public CFrame GetNodeLocalCFrame() => throw new InvalidOperationException("Cannot call reserved method for RobloxCS transpiler.");
    public CFrame GetNodeWorldCFrame() => throw new InvalidOperationException("Cannot call reserved method for RobloxCS transpiler.");
    public CFrame GetRawFinalTarget() => throw new InvalidOperationException("Cannot call reserved method for RobloxCS transpiler.");
    public CFrame GetSmoothedFinalTarget() => throw new InvalidOperationException("Cannot call reserved method for RobloxCS transpiler.");
}
