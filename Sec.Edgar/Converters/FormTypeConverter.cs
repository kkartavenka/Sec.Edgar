using System;
using System.Linq;
using Sec.Edgar.Enums;
using Sec.Edgar.Models;
#if NET6_0_OR_GREATER
using System.Text.Json;
using System.Text.Json.Serialization;
#elif NETSTANDARD2_0
using Newtonsoft.Json;
#endif

namespace Sec.Edgar.Converters
{
    internal class FormTypeConverter : JsonConverter<FormType>
    {
#if NET6_0_OR_GREATER
        public override FormType Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
        {
            if (reader.TokenType != JsonTokenType.String)
            {
                throw new JsonException("Unexpected JsonTokenType");
            }
            var value = reader.GetString();
#elif NETSTANDARD2_0
        public override FormType ReadJson(JsonReader reader, Type objectType, FormType existingValue, bool hasExistingValue,
            JsonSerializer serializer)
        {
            if (reader.TokenType != JsonToken.String)
            {
                throw new JsonException("Unexpected JsonTokenType");
            }

            var value = (string)reader.Value;
#endif
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

#if NET6_0_OR_GREATER
        public override void Write(Utf8JsonWriter writer, FormType value, JsonSerializerOptions options)
#elif NETSTANDARD2_0
        public override void WriteJson(JsonWriter writer, FormType value, JsonSerializer serializer)
#endif
        {
            throw new NotImplementedException();
        }
    }
}