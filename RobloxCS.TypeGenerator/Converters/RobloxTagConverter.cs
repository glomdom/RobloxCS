using System.Text.Json;
using System.Text.Json.Serialization;
using RobloxCS.TypeGenerator.Models;

namespace RobloxCS.TypeGenerator.Converters;

public class RobloxTagConverter : JsonConverter<RobloxTag> {
    public override RobloxTag Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options) {
        var result = new RobloxTag();

        switch (reader.TokenType) {
            case JsonTokenType.String: {
                var enumStr = ReadString(ref reader);
                if (Enum.TryParse<RobloxTagKind>(enumStr, true, out var kind)) {
                    result.EnumValue = kind;
                } else {
                    throw new JsonException($"Unknown RobloxTagKind: {enumStr}");
                }

                break;
            }

            case JsonTokenType.StartObject: result.ComplexValue = JsonSerializer.Deserialize<RobloxComplexTag>(ref reader, options); break;

            default: throw new JsonException($"Unexpected token: {reader.TokenType}");
        }

        return result;
    }

    public override void Write(Utf8JsonWriter writer, RobloxTag value, JsonSerializerOptions options) {
        if (value.IsEnum) {
            writer.WriteStringValue(value.EnumValue.ToString());
        } else if (value.ComplexValue != null) {
            JsonSerializer.Serialize(writer, value.ComplexValue, options);
        }
    }

    private static string ReadString(ref Utf8JsonReader reader) {
        var result = reader.GetString();

        return result ?? throw new JsonException("Failed to read string. Is it missing?");
    }
}