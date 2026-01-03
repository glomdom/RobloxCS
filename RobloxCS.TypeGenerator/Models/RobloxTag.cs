using System.Text.Json.Serialization;
using RobloxCS.TypeGenerator.Converters;

namespace RobloxCS.TypeGenerator.Models;

[JsonConverter(typeof(RobloxTagConverter))]
public class RobloxTag {
    public RobloxTagKind? EnumValue { get; set; }
    public RobloxComplexTag? ComplexValue { get; set; }

    public bool IsEnum => EnumValue.HasValue;
    public bool IsComplex => ComplexValue is not null;

    public override string ToString() {
        if (IsEnum) return $"{EnumValue}";
        if (IsComplex) return $"preferredDescriptor={ComplexValue!.PreferredDescriptorName}, threadSafety={ComplexValue!.ThreadSafety}";

        return null!; // will not happen
    }
}