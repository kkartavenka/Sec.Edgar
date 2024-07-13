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
    internal class BoolArrayConverter : JsonConverter<bool[]>
    {
#if NET6_0_OR_GREATER
        public override bool[] Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
        {
            if (reader.TokenType != JsonTokenType.StartArray)
            {
                throw new JsonException("Unexpected JsonTokenType");
            }

            reader.Read();

            var elements = new List<bool>();

            while (reader.TokenType != JsonTokenType.EndArray)
            {
                reader.TryGetInt32(out var value);
                reader.Read();
                elements.Add(value == 1);
            }

            return elements.ToArray();
        }
#elif NETSTANDARD2_0
        public override bool[] ReadJson(JsonReader reader, Type objectType, bool[] existingValue, bool hasExistingValue,
            JsonSerializer serializer)
        {
            if (reader.TokenType != JsonToken.StartArray)
            {
                throw new JsonException($"Expected JsonToken {JsonToken.StartArray}, observed {reader.TokenType}");
            }
            
            var elements = new List<bool>();
            reader.Read();
            while (reader.TokenType != JsonToken.EndArray)
            {
                if (reader.Value != null)
                {
                    switch (reader.TokenType)
                    {
                        case JsonToken.Boolean:
                            elements.Add((bool)reader.Value);
                            break;
                        case JsonToken.String:
                            if (bool.TryParse((string)reader.Value, out var val))
                            {
                                elements.Add(val);
                            }
                            break;
                        case JsonToken.Integer:
                            elements.Add((long)reader.Value == 1);
                            break;
                    }
                }

                reader.Read();
            }

            return elements.ToArray();
        }
#endif
        
#if NET6_0_OR_GREATER
        public override void Write(Utf8JsonWriter writer, bool[] value, JsonSerializerOptions options)
#elif NETSTANDARD2_0
        public override void WriteJson(JsonWriter writer, bool[] value, JsonSerializer serializer)
#endif
        {
            throw new NotImplementedException();
        }
    }
}