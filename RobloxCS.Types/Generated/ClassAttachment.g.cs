#nullable enable
namespace RobloxCS.Types;
[RobloxNative("Attachment", RobloxNativeType.Instance)]
public partial class Attachment : Instance  {
    public Vector3 Axis { get; } = default!;
    public CFrame CFrame { get; } = default!;
    public Vector3 SecondaryAxis { get; } = default!;
    public bool Visible { get; } = default!;
    public Vector3 WorldAxis { get; } = default!;
    public CFrame WorldCFrame { get; } = default!;
    public Vector3 WorldSecondaryAxis { get; } = default!;
    public List<Instance> GetConstraints() => throw new InvalidOperationException("Cannot call reserved method for RobloxCS transpiler.");
}
