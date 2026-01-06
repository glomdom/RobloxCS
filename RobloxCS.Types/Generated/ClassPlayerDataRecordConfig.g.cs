#nullable enable
namespace RobloxCS.Types;
[RobloxNative("PlayerDataRecordConfig", RobloxNativeType.Instance)]
public partial class PlayerDataRecordConfig : Instance  {
    public string RecordName { get; set; } = default!;
    public object GetDefaultValue() => throw new InvalidOperationException("Cannot call reserved method for RobloxCS transpiler.");
    public void SetDefaultValue() => throw new InvalidOperationException("Cannot call reserved method for RobloxCS transpiler.");
}
