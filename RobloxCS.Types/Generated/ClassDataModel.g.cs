#nullable enable
namespace RobloxCS.Types;
[RobloxNative("DataModel", RobloxNativeType.Instance)]
public partial class DataModel : ServiceProvider  {
    public int CreatorId { get; set; } = default!;
    public Enums.CreatorType CreatorType { get; set; } = default!;
    public int GameId { get; set; } = default!;
    public Enums.Genre Genre { get; set; } = default!;
    public string JobId { get; set; } = default!;
    public Enums.MatchmakingType MatchmakingType { get; set; } = default!;
    public int PlaceId { get; set; } = default!;
    public int PlaceVersion { get; set; } = default!;
    public string PrivateServerId { get; set; } = default!;
    public int PrivateServerOwnerId { get; set; } = default!;
    public Workspace Workspace { get; set; } = default!;
    public void BindToClose() => throw new InvalidOperationException("Cannot call reserved method for RobloxCS transpiler.");
    public bool IsLoaded() => throw new InvalidOperationException("Cannot call reserved method for RobloxCS transpiler.");
    public RBXScriptSignal<bool> GraphicsQualityChangeRequest { get; private set; } = null!;
    public RBXScriptSignal Loaded { get; private set; } = null!;
}
