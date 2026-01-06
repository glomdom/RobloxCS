#nullable enable
namespace RobloxCS.Types;
[RobloxNative("AudioSearchParams", RobloxNativeType.Instance)]
public partial class AudioSearchParams : Instance  {
    public string Album { get; set; } = default!;
    public string Artist { get; set; } = default!;
    public Enums.AudioSubType AudioSubType { get; set; } = default!;
    public int MaxDuration { get; set; } = default!;
    public int MinDuration { get; set; } = default!;
    public string SearchKeyword { get; set; } = default!;
    public string Tag { get; set; } = default!;
    public string Title { get; set; } = default!;
}
