#nullable enable
namespace RobloxCS.Types;
[RobloxNative("RenderingTest", RobloxNativeType.Instance)]
public partial class RenderingTest : Instance  {
    public CFrame CFrame { get; set; } = default!;
    public int ComparisonDiffThreshold { get; set; } = default!;
    public Enums.RenderingTestComparisonMethod ComparisonMethod { get; set; } = default!;
    public float ComparisonPsnrThreshold { get; set; } = default!;
    public string Description { get; set; } = default!;
    public float FieldOfView { get; set; } = default!;
    public bool PerfTest { get; set; } = default!;
    public bool QualityAuto { get; set; } = default!;
    public int QualityLevel { get; set; } = default!;
    public int RenderingTestFrameCount { get; set; } = default!;
    public bool ShouldSkip { get; set; } = default!;
    public string Ticket { get; set; } = default!;
    public int Timeout { get; set; } = default!;
    public void RenderdocTriggerCapture() => ThrowHelper.ThrowTranspiledMethod();
}
