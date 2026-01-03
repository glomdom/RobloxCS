#nullable enable
namespace RobloxCS.Types;
[RobloxNative("RenderingTest", RobloxNativeType.Instance)]
public partial class RenderingTest : Instance  {
    public CFrame CFrame { get; } = default!;
    public int ComparisonDiffThreshold { get; } = default!;
    public Enums.RenderingTestComparisonMethod ComparisonMethod { get; } = default!;
    public float ComparisonPsnrThreshold { get; } = default!;
    public string Description { get; } = default!;
    public float FieldOfView { get; } = default!;
    public bool PerfTest { get; } = default!;
    public bool QualityAuto { get; } = default!;
    public int QualityLevel { get; } = default!;
    public int RenderingTestFrameCount { get; } = default!;
    public bool ShouldSkip { get; } = default!;
    public string Ticket { get; } = default!;
    public int Timeout { get; } = default!;
    public void RenderdocTriggerCapture() => throw new InvalidOperationException("Cannot call reserved method for RobloxCS transpiler.");
}
