using System.Text.Json;
using System.Text.Json.Serialization;
using RobloxCS.TypeGenerator.Models;

namespace RobloxCS.TypeGenerator.Converters;

public class RobloxSecurityConverter : JsonConverter<RobloxSecurity> {
    public override RobloxSecurity? Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options) {
        switch (reader.TokenType) {
            case JsonTokenType.Null: return null;
            case JsonTokenType.String: {
                var val = reader.GetString();
                if (val is null) throw new JsonException("Could not read security string.");

                var enumVal = ParseEnum(val);

                return new RobloxSecurity {
                    Read = enumVal,
                    Write = enumVal,
                };
            }

            case JsonTokenType.StartObject: {
                RobloxSecurityType? read = null;
                RobloxSecurityType? write = null;

                while (reader.Read()) {
                    switch (reader.TokenType) {
                        case JsonTokenType.EndObject when !read.HasValue || !write.HasValue: throw new JsonException("Missing Read/Write keys in Security object.");
                        case JsonTokenType.EndObject:
                            return new RobloxSecurity {
                                Read = read.Value,
                                Write = write.Value,
                            };

                        case JsonTokenType.PropertyName: {
                            var propName = ReadString(ref reader);
                            reader.Read();

                            switch (propName) {
                                case "Read": read = ParseEnum(ReadString(ref reader)); break;
                                case "Write": write = ParseEnum(ReadString(ref reader)); break;
                            }

                            break;
                        }

                        default: throw new JsonException($"Unexpected token inside Security object: '{reader.TokenType}'.");
                    }
                }

                throw new JsonException($"Unexpected token type for Security: '{reader.TokenType}'");
            }

            default: throw new JsonException($"Unexpected token for Security: '{reader.TokenType}'.");
        }
    }

    public override void Write(Utf8JsonWriter writer, RobloxSecurity value, JsonSerializerOptions options) {
        JsonSerializer.Serialize(writer, value, value.GetType(), options);
    }

    private static string ReadString(ref Utf8JsonReader reader) {
        var result = reader.GetString();

        return result ?? throw new JsonException("Failed to read string. Is it missing?");
    }

    private static RobloxSecurityType ParseEnum(string value) {
        return Enum.TryParse<RobloxSecurityType>(value, out var result)
            ? result
            : throw new JsonException("Invalid Security type provided.");
    }
}