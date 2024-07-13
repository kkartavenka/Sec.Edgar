using System;
using System.Collections.Generic;
using Sec.Edgar.Models.Edgar;
#if NET6_0_OR_GREATER
using System.Text.Json;
using System.Text.Json.Serialization;
#elif NETSTANDARD2_0
using Newtonsoft.Json;
#endif

namespace Sec.Edgar.Converters
{
    internal class DictionaryConverter : JsonConverter<Dictionary<string, FactModelJsonDto>>
    {
#if NET6_0_OR_GREATER
        public override Dictionary<string, FactModelJsonDto> Read(ref Utf8JsonReader reader, Type typeToConvert,
            JsonSerializerOptions options)
        {
            if (reader.TokenType != JsonTokenType.StartObject)
            {
                throw new JsonException("Unexpected JsonTokenType");
            }

            var returnVar = new Dictionary<string, FactModelJsonDto>();
            while (reader.Read())
            {
                if (reader.TokenType == JsonTokenType.EndObject)
                {
                    return returnVar;
                }

                if (reader.TokenType != JsonTokenType.PropertyName)
                {
                    throw new JsonException("Unexpected JsonTokenType");
                }

                var propertyName = reader.GetString();

                if (string.IsNullOrWhiteSpace(propertyName))
                {
                    throw new JsonException();
                }

                reader.Read();
                var value = JsonSerializer.Deserialize<FactModelJsonDto>(ref reader, options);

                returnVar.Add(propertyName, value);
            }

            return returnVar;
        }
#endif
        
#if NETSTANDARD2_0
        public override Dictionary<string, FactModelJsonDto> ReadJson(JsonReader reader, Type objectType, Dictionary<string, FactModelJsonDto> existingValue, bool hasExistingValue,
            JsonSerializer serializer)
        {
            if (reader.TokenType != JsonToken.StartObject)
            {
                throw new JsonException($"Expected JsonToken {JsonToken.StartArray}, observed {reader.TokenType}");
            }

            var returnVar = new Dictionary<string, FactModelJsonDto>();
            while (reader.Read())
            {
                if (reader.TokenType == JsonToken.EndObject)
                {
                    return returnVar;
                }

                if (reader.TokenType != JsonToken.PropertyName)
                {
                    throw new JsonException("Unexpected JsonTokenType");
                }

                var propertyName = (string)reader.Value;

                if (string.IsNullOrWhiteSpace(propertyName))
                {
                    throw new JsonException();
                }

                reader.Read();
                var jsonDeserializer = new JsonSerializer();
                var value = jsonDeserializer.Deserialize<FactModelJsonDto>(reader);

                returnVar.Add(propertyName, value);
            }

            return returnVar;
        }
#endif

#if NET6_0_OR_GREATER
        public override void Write(Utf8JsonWriter writer, Dictionary<string, FactModelJsonDto> value,
            JsonSerializerOptions options)
#elif NETSTANDARD2_0
        public override void WriteJson(JsonWriter writer, Dictionary<string, FactModelJsonDto> value, JsonSerializer serializer)
#endif
        {
            throw new NotImplementedException();
        }
    }
}