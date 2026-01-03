#nullable enable
namespace RobloxCS.Types;
[RobloxNative("IKControl", RobloxNativeType.Instance)]
public partial class IKControl : Instance  {
    public Instance ChainRoot { get; } = default!;
    public bool Enabled { get; } = default!;
    public Instance EndEffector { get; } = default!;
    public CFrame EndEffectorOffset { get; } = default!;
    public CFrame Offset { get; } = default!;
    public Instance Pole { get; } = default!;
    public int Priority { get; } = default!;
    public float SmoothTime { get; } = default!;
    public Instance Target { get; } = default!;
    public Enums.IKControlType Type { get; } = default!;
    public float Weight { get; } = default!;
    public int GetChainCount() => throw new InvalidOperationException("Cannot call reserved method for RobloxCS transpiler.");
    public float GetChainLength() => throw new InvalidOperationException("Cannot call reserved method for RobloxCS transpiler.");
    public CFrame GetNodeLocalCFrame() => throw new InvalidOperationException("Cannot call reserved method for RobloxCS transpiler.");
    public CFrame GetNodeWorldCFrame() => throw new InvalidOperationException("Cannot call reserved method for RobloxCS transpiler.");
    public CFrame GetRawFinalTarget() => throw new InvalidOperationException("Cannot call reserved method for RobloxCS transpiler.");
    public CFrame GetSmoothedFinalTarget() => throw new InvalidOperationException("Cannot call reserved method for RobloxCS transpiler.");
}
