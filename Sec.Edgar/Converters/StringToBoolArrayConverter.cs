using System.Text.Json;
using System.Text.Json.Serialization;

namespace Sec.Edgar.Converters;

internal class StringToBoolArrayConverter : JsonConverter<bool[]>
{
    public override bool[] Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
    {
        if (reader.TokenType != JsonTokenType.StartArray)
        {
            throw new JsonException();
        }
        reader.Read();

        var elements = new List<bool>();

        while (reader.TokenType != JsonTokenType.EndArray)
        {
            reader.TryGetInt32(out var value);
            elements.Add(value == 1);
            reader.Read();
        }

        return elements.ToArray();
    }

    public override void Write(Utf8JsonWriter writer, bool[] value, JsonSerializerOptions options)
    {
        throw new NotImplementedException();
    }
}