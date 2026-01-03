#nullable enable
namespace RobloxCS.Types;
[RobloxNative("VideoSampler", RobloxNativeType.Instance)]
public partial class VideoSampler : Object  {
    public double TimeLength { get; } = default!;
    public string VideoContent { get; } = default!;
    public List<object> GetSamplesAtTimesAsync() => throw new InvalidOperationException("Cannot call reserved method for RobloxCS transpiler.");
}
