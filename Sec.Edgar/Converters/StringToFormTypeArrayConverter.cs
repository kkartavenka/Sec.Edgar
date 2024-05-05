using System.Text.Json;
using System.Text.Json.Serialization;
using Sec.Edgar.Enums;
using Sec.Edgar.Models;

namespace Sec.Edgar.Converters;

internal class StringToFormTypeArrayConverter : JsonConverter<FormType[]>
{
    public override FormType[] Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
    {
        if (reader.TokenType != JsonTokenType.StartArray)
        {
            throw new JsonException("Unexpected JsonTokenType");
        }
        reader.Read();
        
        var elements = new List<FormType>();
        while (reader.TokenType != JsonTokenType.EndArray)
        {
            var value = reader.GetString();
            
            if (string.IsNullOrWhiteSpace(value))
            {
                elements.Add(FormType.Unrecognized);
            }
            else
            {
                var matchByAttribute = EnumExtension.GetMapping<FormType>()
                    .SingleOrDefault(x => x.Key.Equals(value, StringComparison.CurrentCultureIgnoreCase));

                elements.Add(Enum.TryParse(matchByAttribute.Value, out FormType observedType)
                    ? observedType
                    : FormType.Unrecognized);
            }

            reader.Read();
        }

        return elements.ToArray();
    }

    public override void Write(Utf8JsonWriter writer, FormType[] value, JsonSerializerOptions options)
    {
        throw new NotImplementedException();
    }
}