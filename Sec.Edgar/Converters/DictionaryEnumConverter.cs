using System;
using System.Collections.Generic;
using System.Linq;
using Sec.Edgar.Enums;
using Sec.Edgar.Models;
using Sec.Edgar.Models.Edgar;
#if NET6_0_OR_GREATER
using System.Text.Json;
using System.Text.Json.Serialization;
#elif NETSTANDARD2_0
using Newtonsoft.Json;
#endif

namespace Sec.Edgar.Converters
{
    internal class DictionaryEnumConverter : JsonConverter<Dictionary<Taxonomy, Dictionary<string, FactModelJsonDto>>>
    {
        private readonly DictionaryConverter _jsonConverter = new DictionaryConverter();

#if NET6_0_OR_GREATER
        public override Dictionary<Taxonomy, Dictionary<string, FactModelJsonDto>> Read(ref Utf8JsonReader reader,
            Type typeToConvert, JsonSerializerOptions options)
        {
            if (reader.TokenType != JsonTokenType.StartObject)
            {
                throw new JsonException("Unexpected JsonTokenType");
            }

            var returnVar = new Dictionary<Taxonomy, Dictionary<string, FactModelJsonDto>>();
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

                var matchByAttribute = EnumExtension.GetMapping<Taxonomy>()
                    .SingleOrDefault(x => x.Key.Equals(propertyName, StringComparison.CurrentCultureIgnoreCase));

                var parsedKey = Enum.TryParse(matchByAttribute.Value, out Taxonomy observedType)
                    ? observedType
                    : Taxonomy.Unrecognized;

                reader.Read();
                var value = _jsonConverter.Read(ref reader, null, options)!;

                returnVar.Add(parsedKey, value);
            }

            return returnVar;
        }
        
        public override void Write(Utf8JsonWriter writer, Dictionary<Taxonomy, Dictionary<string, FactModelJsonDto>> value,
            JsonSerializerOptions options)
        {
            throw new NotImplementedException();
        }
#elif NETSTANDARD2_0
        public override Dictionary<Taxonomy, Dictionary<string, FactModelJsonDto>> ReadJson(JsonReader reader, Type objectType, Dictionary<Taxonomy, Dictionary<string, FactModelJsonDto>> existingValue, bool hasExistingValue,
            JsonSerializer serializer)
        {
            if (reader.TokenType != JsonToken.StartObject)
            {
                throw new JsonException("Unexpected JsonToken");
            }

            var returnVar = new Dictionary<Taxonomy, Dictionary<string, FactModelJsonDto>>();
            while (reader.Read())
            {
                if (reader.TokenType == JsonToken.EndObject)
                {
                    return returnVar;
                }

                if (reader.TokenType != JsonToken.PropertyName)
                {
                    throw new JsonException("Unexpected JsonToken");
                }

                var propertyName = (string)reader.Value;

                var matchByAttribute = EnumExtension.GetMapping<Taxonomy>()
                    .SingleOrDefault(x => x.Key.Equals(propertyName, StringComparison.CurrentCultureIgnoreCase));

                var parsedKey = Enum.TryParse(matchByAttribute.Value, out Taxonomy observedType)
                    ? observedType
                    : Taxonomy.Unrecognized;

                reader.Read();
                var value = _jsonConverter.ReadJson(reader, null, null, false, serializer);

                returnVar.Add(parsedKey, value);
            }

            return returnVar;
        }

        public override void WriteJson(JsonWriter writer, Dictionary<Taxonomy, Dictionary<string, FactModelJsonDto>> value, JsonSerializer serializer)
        {
            throw new NotImplementedException();
        }
#endif
    }
}