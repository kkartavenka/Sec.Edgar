using System.Text.Json;
using System.Text.Json.Serialization;
using Sec.Edgar.Enums;
using Sec.Edgar.Models;

namespace Sec.Edgar.Converters;

internal class StringToFormTypeConverter : JsonConverter<FormType>
{
    public override FormType Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
    {
        if (reader.TokenType != JsonTokenType.String)
        {
            throw new JsonException("Unexpected JsonTokenType");
        }

        var value = reader.GetString();

        if (string.IsNullOrWhiteSpace(value))
        {
            return FormType.Unrecognized;
        }

        var matchByAttribute = EnumExtension.GetMapping<FormType>()
            .SingleOrDefault(x => x.Key.Equals(value, StringComparison.CurrentCultureIgnoreCase));

        return Enum.TryParse(matchByAttribute.Value, out FormType observedType)
            ? observedType
            : FormType.Unrecognized;
    }

    public override void Write(Utf8JsonWriter writer, FormType value, JsonSerializerOptions options)
    {
        throw new NotImplementedException();
    }
}