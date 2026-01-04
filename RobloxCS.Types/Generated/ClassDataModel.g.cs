#nullable enable
namespace RobloxCS.Types;
[RobloxNative("DataModel", RobloxNativeType.Instance)]
public partial class DataModel : ServiceProvider  {
    public int CreatorId { get; } = default!;
    public Enums.CreatorType CreatorType { get; } = default!;
    public int GameId { get; } = default!;
    public Enums.Genre Genre { get; } = default!;
    public string JobId { get; } = default!;
    public Enums.MatchmakingType MatchmakingType { get; } = default!;
    public int PlaceId { get; } = default!;
    public int PlaceVersion { get; } = default!;
    public string PrivateServerId { get; } = default!;
    public int PrivateServerOwnerId { get; } = default!;
    public Workspace Workspace { get; } = default!;
    public void BindToClose() => throw new InvalidOperationException("Cannot call reserved method for RobloxCS transpiler.");
    public bool IsLoaded() => throw new InvalidOperationException("Cannot call reserved method for RobloxCS transpiler.");
    public RBXScriptSignal<bool> GraphicsQualityChangeRequest { get; private set; } = null!;
    public RBXScriptSignal Loaded { get; private set; } = null!;
}
