using System.Text.Json;
using System.Text.Json.Serialization;
using Sec.Edgar.Models;

namespace Sec.Edgar.Converters;

public class StringToEnumConverter<T> : JsonConverter<T> where T: struct, Enum
{
    public override T Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
    {
        if (reader.TokenType != JsonTokenType.String)
        {
            throw new JsonException("Unexpected JsonTokenType");
        }

        var value = reader.GetString();
        if (string.IsNullOrWhiteSpace(value))
        {
            return default;
        }
        
        var matchByAttribute = EnumExtension.GetMapping<T>()
            .SingleOrDefault(x => x.Key.Equals(value, StringComparison.CurrentCultureIgnoreCase));

        return Enum.TryParse(matchByAttribute.Value, out T observedType)
            ? observedType
            : default;
    }

    public override void Write(Utf8JsonWriter writer, T value, JsonSerializerOptions options)
    {
        throw new NotImplementedException();
    }
}