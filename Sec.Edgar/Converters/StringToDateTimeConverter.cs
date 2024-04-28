using System.Text.Json;
using System.Text.Json.Serialization;

namespace Sec.Edgar.Converters;

public class StringToDateTimeConverter : JsonConverter<DateTime?>
{
    public override DateTime? Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
    {
        var sDateTime = reader.GetString();
        if (string.IsNullOrWhiteSpace(sDateTime))
        {
            return null;
        }

        if (DateTime.TryParse(reader.GetString(), out var parsedResult))
        {
            return parsedResult;
        }

        return null;
    }

    public override void Write(Utf8JsonWriter writer, DateTime? value, JsonSerializerOptions options)
    {
        throw new NotImplementedException();
    }
}