#if NET7_0_OR_GREATER
using System.Numerics;
#endif
#if NET6_0_OR_GREATER
using System.Text.Json;
using System.Text.Json.Serialization;
#elif NETSTANDARD2_0
using Newtonsoft.Json;
#endif
using System;

namespace Sec.Edgar.Converters
{
#if NET6_0_OR_GREATER
    internal class NumericConverter<T> : JsonConverter<T> where T : IConvertible
#elif NETSTANDARD2_0
    internal class NumericConverter<T> : JsonConverter<T> where T : IConvertible
#endif
    {
#if NET6_0_OR_GREATER
        public override T Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
        {
            if (reader.TokenType == JsonTokenType.Number)
            {
                if (reader.TryGetInt64(out var intVal))
                {
                    return (T)Convert.ChangeType(intVal, typeof(T));
                }

                return (T)Convert.ChangeType(reader.GetDouble(), typeof(T));
            }

            if (reader.TokenType != JsonTokenType.String)
            {
                throw new JsonException($"Unexpected JsonTokenType {reader.TokenType}, expected {JsonTokenType.Number} or {JsonTokenType.String}");
            }
            
            var strVal = reader.GetString();
            return (T)Convert.ChangeType(strVal, typeof(T));
        }

        public override void Write(Utf8JsonWriter writer, T value, JsonSerializerOptions options)
        {
            throw new NotImplementedException();
        }
#endif

#if NETSTANDARD2_0
        public override void WriteJson(JsonWriter writer, T value, JsonSerializer serializer)
        {
            throw new NotImplementedException();
        }

        public override T ReadJson(JsonReader reader, Type objectType, T existingValue, bool hasExistingValue,
            JsonSerializer serializer)
        {
            if (reader.Value == null)
            {
                return default;
            }
            
            if (reader.TokenType == JsonToken.Integer)
            {
                var intVal = (long)reader.Value;
                return (T)Convert.ChangeType(intVal, typeof(T));
            }

            if (reader.TokenType == JsonToken.Float)
            {
                var dblVal = (double)reader.Value;
                return (T)Convert.ChangeType(dblVal, typeof(T));
            }
            
            var strVal = (string)reader.Value;
            if (string.IsNullOrEmpty(strVal))
            {
                throw new JsonException("The string is null or empty");
            }

            return (T)Convert.ChangeType(strVal, typeof(T));
        }
#endif
    }
}