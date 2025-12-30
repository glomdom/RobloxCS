using System.Text.Json;
using System.Text.Json.Nodes;
using System.Text.Json.Serialization;
using RobloxCS.TypeGenerator.Models;

namespace RobloxCS.TypeGenerator.Converters;

public class RobloxMemberConverter : JsonConverter<RobloxMember> {
    public override RobloxMember? Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options) {
        var node = JsonNode.Parse(ref reader);
        if (node == null) return null;

        var typeVal = (string?)node["MemberType"] ?? throw new JsonException("Member missing MemberType");

        return typeVal switch {
            "Property" => node.Deserialize<RobloxProperty>(options),
            
            _ => null,
        };
    }

    public override void Write(Utf8JsonWriter writer, RobloxMember value, JsonSerializerOptions options) {
        JsonSerializer.Serialize(writer, value, value.GetType(), options);
    }
}