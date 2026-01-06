#nullable enable
namespace RobloxCS.Types;
[RobloxNative("PlayerDataRecord", RobloxNativeType.Instance)]
public partial class PlayerDataRecord : Instance  {
    public int CreatedTime { get; set; } = default!;
    public bool DefaultRecordName { get; set; } = default!;
    public bool Dirty { get; set; } = default!;
    public Enums.PlayerDataErrorState Error { get; set; } = default!;
    public int FlushedTime { get; set; } = default!;
    public int LoadedTime { get; set; } = default!;
    public int ModifiedTime { get; set; } = default!;
    public bool NewRecord { get; set; } = default!;
    public bool Readable { get; set; } = default!;
    public string RecordName { get; set; } = default!;
    public bool Writable { get; set; } = default!;
    public Player GetPlayer() => throw new InvalidOperationException("Cannot call reserved method for RobloxCS transpiler.");
    public object GetValue() => throw new InvalidOperationException("Cannot call reserved method for RobloxCS transpiler.");
    public RBXScriptSignal GetValueChangedSignal() => throw new InvalidOperationException("Cannot call reserved method for RobloxCS transpiler.");
    public void RemoveValue() => throw new InvalidOperationException("Cannot call reserved method for RobloxCS transpiler.");
    public void SetValue() => throw new InvalidOperationException("Cannot call reserved method for RobloxCS transpiler.");
    public void ReleaseAsync() => throw new InvalidOperationException("Cannot call reserved method for RobloxCS transpiler.");
    public void RequestFlushAsync() => throw new InvalidOperationException("Cannot call reserved method for RobloxCS transpiler.");
    new public RBXScriptSignal<string, object> Changed { get; private set; } = null!;
    public RBXScriptSignal<bool, string?> Flushed { get; private set; } = null!;
    public RBXScriptSignal<bool, string?> Loaded { get; private set; } = null!;
}
