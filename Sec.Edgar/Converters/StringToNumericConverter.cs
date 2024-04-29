using System.Numerics;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace Sec.Edgar.Converters;

internal class StringToNumericConverter<T> : JsonConverter<T> where T : INumber<T>, IConvertible
{
    public override T? Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
    {
        if (reader.TokenType == JsonTokenType.Number)
        {
            if (reader.TryGetInt64(out var intVal))
            {
                return (T)Convert.ChangeType(intVal, typeof(T));
            }

            return (T)Convert.ChangeType(reader.GetDouble(), typeof(T));
        }
        
        var strVal = reader.GetString();
        if (strVal is null)
        {
            throw new JsonException("Unexpected JsonTokenType");
        }
        
        return (T)Convert.ChangeType(strVal, typeof(T));
    }

    public override void Write(Utf8JsonWriter writer, T value, JsonSerializerOptions options)
    {
        throw new NotImplementedException();
    }
}