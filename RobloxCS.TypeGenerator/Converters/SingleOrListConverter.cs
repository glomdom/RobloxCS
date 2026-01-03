using System.Text.Json;
using System.Text.Json.Serialization;

namespace RobloxCS.TypeGenerator.Converters;

public sealed class SingleOrListConverter<T> : JsonConverter<List<T>> {
    public override List<T>? Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options) {
        if (reader.TokenType == JsonTokenType.StartArray) {
            return JsonSerializer.Deserialize<List<T>>(ref reader, options);
        }

        var single = JsonSerializer.Deserialize<T>(ref reader, options);
        if (single is null) throw new JsonException("Failed to parse element.");

        return [single];
    }

    public override void Write(Utf8JsonWriter writer, List<T> value, JsonSerializerOptions options) {
        JsonSerializer.Serialize(writer, value, value.GetType(), options);
    }
}