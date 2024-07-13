using System;
using System.Collections.Generic;
using Sec.Edgar.Enums;
#if NET6_0_OR_GREATER
using System.Text.Json;
using System.Text.Json.Serialization;
#elif NETSTANDARD2_0
using Newtonsoft.Json;
#endif

namespace Sec.Edgar.Converters
{
    internal class ExchangeTypeArrayConverter : JsonConverter<ExchangeType[]>
    {
        private const string MatchNyse = "nyse";
        private const string MatchNasdaq = "nasdaq";
        private const string MatchEuronext = "euronext";
        private const string MatchOtc = "otc";
        
#if NET6_0_OR_GREATER
        public override ExchangeType[] Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
        {
            if (reader.TokenType != JsonTokenType.StartArray)
            {
                throw new JsonException("Unexpected JsonTokenType");
            }

            reader.Read();

            var elements = new List<ExchangeType>();
            while (reader.TokenType != JsonTokenType.EndArray)
            {
                var value = reader.GetString();
                PopulateList(elements, value);
                reader.Read();
            }

            return elements.ToArray();
        }
#elif NETSTANDARD2_0
        public override ExchangeType[] ReadJson(JsonReader reader, Type objectType, ExchangeType[] existingValue, bool hasExistingValue,
            JsonSerializer serializer)
        {
            if (reader.TokenType != JsonToken.StartArray)
            {
                throw new JsonException($"Expected {JsonToken.StartArray}, observed {reader.TokenType}");
            }

            var elements = new List<ExchangeType>();
            while (reader.TokenType != JsonToken.EndArray)
            {
                if (reader.TokenType == JsonToken.String)
                {
                    var value = (string)reader.Value;
                    PopulateList(elements, value);
                }

                reader.Read();
            }

            return elements.ToArray();
        }
#endif

#if NET6_0_OR_GREATER
        public override void Write(Utf8JsonWriter writer, ExchangeType[] value, JsonSerializerOptions options)
        {
            throw new NotImplementedException();
        }
#elif NETSTANDARD2_0
        public override void WriteJson(JsonWriter writer, ExchangeType[] value, JsonSerializer serializer)
        {
            throw new NotImplementedException();
        }
#endif

        private void PopulateList(List<ExchangeType> exportList, string value)
        {
            if (string.IsNullOrEmpty(value))
            {
                exportList.Add(ExchangeType.Empty);
                return;
            }
            
            switch (value.ToLower().Trim())
            {
                case MatchNasdaq:
                    exportList.Add(ExchangeType.Nasdaq);
                    break;
                case MatchNyse:
                    exportList.Add(ExchangeType.NYSE);
                    break;
                case MatchEuronext:
                    exportList.Add(ExchangeType.Euronext);
                    break;
                case MatchOtc:
                    exportList.Add(ExchangeType.OTC);
                    break;
                default:
                    exportList.Add(ExchangeType.Unknown);
                    break;
            }
        }
    }
}