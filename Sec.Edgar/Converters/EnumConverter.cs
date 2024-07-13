using System;
using System.Linq;
using Sec.Edgar.Models;
#if NET6_0_OR_GREATER
using System.Text.Json;
using System.Text.Json.Serialization;
#elif NETSTANDARD2_0
using Newtonsoft.Json;
#endif

namespace Sec.Edgar.Converters
{
    public class EnumConverter<T> : JsonConverter<T> where T : struct, Enum
    {
#if NET6_0_OR_GREATER
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
#elif NETSTANDARD2_0
        public override void WriteJson(JsonWriter writer, T value, JsonSerializer serializer)
        {
            throw new NotImplementedException();
        }

        public override T ReadJson(JsonReader reader, Type objectType, T existingValue, bool hasExistingValue, JsonSerializer serializer)
        {
            if (reader.TokenType != JsonToken.String)
            {
                throw new JsonException($"Expected JsonToken {JsonToken.StartArray}, observed {reader.TokenType}");
            }

            var value = (string)reader.Value;
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
#endif
    }
}