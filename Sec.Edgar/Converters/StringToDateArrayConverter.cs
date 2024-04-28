using System.Text.Json;
using System.Text.Json.Serialization;

namespace Sec.Edgar.Converters;

internal class StringToDateArrayConverter : JsonConverter<DateTime?[]>
{
    public override DateTime?[] Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
    {
        if (reader.TokenType != JsonTokenType.StartArray)
        {
            throw new JsonException("Unexpected JsonTokenType");
        }
        reader.Read();

        var elements = new List<DateTime?>();

        while (reader.TokenType != JsonTokenType.EndArray)
        {
            var value = reader.GetString();
            
            if (string.IsNullOrWhiteSpace(value))
            {
                elements.Add(null);
            }
            else
            {
                if (DateTime.TryParse(value, out var parsedDateTime))
                {
                    elements.Add(parsedDateTime);
                }
                else
                {
                    elements.Add(null);
                }
            }

            reader.Read();
        }

        return elements.ToArray();
    }

    public override void Write(Utf8JsonWriter writer, DateTime?[] value, JsonSerializerOptions options)
    {
        throw new NotImplementedException();
    }
}