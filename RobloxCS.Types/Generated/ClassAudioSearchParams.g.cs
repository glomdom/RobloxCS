#nullable enable
namespace RobloxCS.Types;
[RobloxNative("AudioSearchParams", RobloxNativeType.Instance)]
public partial class AudioSearchParams : Instance  {
    public string Album { get; } = default!;
    public string Artist { get; } = default!;
    public Enums.AudioSubType AudioSubType { get; } = default!;
    public int MaxDuration { get; } = default!;
    public int MinDuration { get; } = default!;
    public string SearchKeyword { get; } = default!;
    public string Tag { get; } = default!;
    public string Title { get; } = default!;
}
