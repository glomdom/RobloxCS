#nullable enable
namespace RobloxCS.Types;
[RobloxNative("ReflectionMetadataItem", RobloxNativeType.Instance)]
public partial class ReflectionMetadataItem : Instance  {
    public bool Browsable { get; } = default!;
    public string ClassCategory { get; } = default!;
    public bool ClientOnly { get; } = default!;
    public string Constraint { get; } = default!;
    public bool Deprecated { get; } = default!;
    public bool EditingDisabled { get; } = default!;
    public string EditorType { get; } = default!;
    public string FFlag { get; } = default!;
    public bool IsBackend { get; } = default!;
    public int PropertyOrder { get; } = default!;
    public string ScriptContext { get; } = default!;
    public bool ServerOnly { get; } = default!;
    public string SliderScaling { get; } = default!;
    public double UIMaximum { get; } = default!;
    public double UIMinimum { get; } = default!;
    public double UINumTicks { get; } = default!;
}
