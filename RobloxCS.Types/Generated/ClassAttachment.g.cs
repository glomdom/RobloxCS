#nullable enable
namespace RobloxCS.Types;
[RobloxNative("Attachment", RobloxNativeType.Instance)]
public partial class Attachment : Instance  {
    public Vector3 Axis { get; set; } = default!;
    public CFrame CFrame { get; set; } = default!;
    public Vector3 SecondaryAxis { get; set; } = default!;
    public bool Visible { get; set; } = default!;
    public Vector3 WorldAxis { get; set; } = default!;
    public CFrame WorldCFrame { get; set; } = default!;
    public Vector3 WorldSecondaryAxis { get; set; } = default!;
    public List<Instance> GetConstraints() => ThrowHelper.ThrowTranspiledMethod();
}
