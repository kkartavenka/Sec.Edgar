using System;
#if NET6_0_OR_GREATER
using System.Text.Json;
using System.Text.Json.Serialization;
#elif NETSTANDARD2_0
using Newtonsoft.Json;
#endif

namespace Sec.Edgar.Converters
{
    public class DateTimeConverter : JsonConverter<DateTime>
    {
#if NET6_0_OR_GREATER
        public override DateTime Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
        {
            var sDateTime = reader.GetString();
            if (string.IsNullOrWhiteSpace(sDateTime))
            {
                return default;
            }

            if (DateTime.TryParse(reader.GetString(), out var parsedResult))
            {
                return parsedResult;
            }

            return default;
        }
#elif NETSTANDARD2_0
        public override DateTime ReadJson(JsonReader reader, Type objectType, DateTime existingValue, bool hasExistingValue,
            JsonSerializer serializer)
        {
            if (reader.Value == null)
            {
                return default;
            }

            if (reader.TokenType == JsonToken.Date)
            {
                return (DateTime)reader.Value;
            }

            if (reader.TokenType != JsonToken.String)
            {
                throw new JsonException($"Expected JsonToken {JsonToken.String}, observed {reader.TokenType}");
            }
            
            var sDateTime = (string)reader.Value;
            if (string.IsNullOrWhiteSpace(sDateTime))
            {
                return default;
            }

            return DateTime.TryParse((string)reader.Value, out var parsedResult) 
                ? parsedResult 
                : default;
        }
#endif

#if NET6_0_OR_GREATER
        public override void Write(Utf8JsonWriter writer, DateTime value, JsonSerializerOptions options)
#elif NETSTANDARD2_0
        public override void WriteJson(JsonWriter writer, DateTime value, JsonSerializer serializer)
#endif
        {
            throw new NotImplementedException();
        }
    }
}