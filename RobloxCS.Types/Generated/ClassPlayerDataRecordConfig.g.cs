#nullable enable
namespace RobloxCS.Types;
[RobloxNative("PlayerDataRecordConfig", RobloxNativeType.Instance)]
public partial class PlayerDataRecordConfig : Instance  {
    public string RecordName { get; set; } = default!;
    public object GetDefaultValue() => ThrowHelper.ThrowTranspiledMethod();
    public void SetDefaultValue() => ThrowHelper.ThrowTranspiledMethod();
}
