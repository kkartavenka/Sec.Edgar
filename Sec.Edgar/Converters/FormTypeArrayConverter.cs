using System;
using System.Collections.Generic;
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
    internal class FormTypeArrayConverter : JsonConverter<FormType[]>
    {
#if NET6_0_OR_GREATER
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
#elif NETSTANDARD2_0
        public override FormType[] ReadJson(JsonReader reader, Type objectType, FormType[] existingValue, bool hasExistingValue,
            JsonSerializer serializer)
        {
            if (reader.TokenType != JsonToken.StartArray)
            {
                throw new JsonException($"Expected {JsonToken.StartArray}, observed {reader.TokenType}");
            }

            var elements = new List<FormType>();
            while (reader.TokenType != JsonToken.EndArray)
            {
                if (reader.Value != null)
                {
                    var value = (string)reader.Value;

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
                }

                reader.Read();
            }

            return elements.ToArray();
        }
#endif

#if NET6_0_OR_GREATER
        public override void Write(Utf8JsonWriter writer, FormType[] value, JsonSerializerOptions options)
#elif NETSTANDARD2_0
        public override void WriteJson(JsonWriter writer, FormType[] value, JsonSerializer serializer)
#endif
        {
            throw new NotImplementedException();
        }
    }
}