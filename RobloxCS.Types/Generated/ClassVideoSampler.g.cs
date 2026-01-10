#nullable enable
namespace RobloxCS.Types;
[RobloxNative("VideoSampler", RobloxNativeType.Instance)]
public partial class VideoSampler : Object  {
    public double TimeLength { get; set; } = default!;
    public string VideoContent { get; set; } = default!;
    public List<object> GetSamplesAtTimesAsync() => ThrowHelper.ThrowTranspiledMethod();
}
