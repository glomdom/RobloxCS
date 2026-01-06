#nullable enable
namespace RobloxCS.Types;
[RobloxNative("ReflectionMetadataItem", RobloxNativeType.Instance)]
public partial class ReflectionMetadataItem : Instance  {
    public bool Browsable { get; set; } = default!;
    public string ClassCategory { get; set; } = default!;
    public bool ClientOnly { get; set; } = default!;
    public string Constraint { get; set; } = default!;
    public bool Deprecated { get; set; } = default!;
    public bool EditingDisabled { get; set; } = default!;
    public string EditorType { get; set; } = default!;
    public string FFlag { get; set; } = default!;
    public bool IsBackend { get; set; } = default!;
    public int PropertyOrder { get; set; } = default!;
    public string ScriptContext { get; set; } = default!;
    public bool ServerOnly { get; set; } = default!;
    public string SliderScaling { get; set; } = default!;
    public double UIMaximum { get; set; } = default!;
    public double UIMinimum { get; set; } = default!;
    public double UINumTicks { get; set; } = default!;
}
