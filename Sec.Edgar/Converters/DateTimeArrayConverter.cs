using System;
using System.Collections.Generic;
#if NET6_0_OR_GREATER
using System.Text.Json;
using System.Text.Json.Serialization;
#elif NETSTANDARD2_0
using Newtonsoft.Json;
#endif

namespace Sec.Edgar.Converters
{
    internal class DateTimeArrayConverter : JsonConverter<DateTime?[]>
    {
#if NET6_0_OR_GREATER
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
#elif NETSTANDARD2_0        
        public override DateTime?[] ReadJson(JsonReader reader, Type objectType, DateTime?[] existingValue, bool hasExistingValue,
            JsonSerializer serializer)
        {
            if (reader.TokenType != JsonToken.StartArray)
            {
                throw new JsonException($"Expected JsonToken {JsonToken.StartArray}, observed {reader.TokenType}");
            }

            reader.Read();
            var elements = new List<DateTime?>();
            while (reader.TokenType != JsonToken.EndArray)
            {
                if (reader.Value == null)
                {
                    elements.Add(null);
                }
                else switch (reader.TokenType)
                {
                    case JsonToken.Date:
                        elements.Add((DateTime)reader.Value);
                        break;
                    case JsonToken.String when DateTime.TryParse((string)reader.Value, out var parsedDateTime):
                        elements.Add(parsedDateTime);
                        break;
                    case JsonToken.String:
                        elements.Add(null);
                        break;
                    default:
                        throw new JsonException(
                            $"Unexpected JsonToken: {reader.TokenType}, expected {JsonToken.Date} or {JsonToken.String}");
                }

                reader.Read();
            }

            return elements.ToArray();
        }
#endif

#if NET6_0_OR_GREATER
        public override void Write(Utf8JsonWriter writer, DateTime?[] value, JsonSerializerOptions options)
#elif NETSTANDARD2_0
        public override void WriteJson(JsonWriter writer, DateTime?[] value, JsonSerializer serializer)
#endif
        {
            throw new NotImplementedException();
        }
    }
}