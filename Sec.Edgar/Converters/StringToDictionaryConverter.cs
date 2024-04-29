using System.Text.Json;
using System.Text.Json.Serialization;
using Sec.Edgar.Models.Edgar;

namespace Sec.Edgar.Converters;

public class StringToDictionaryConverter : JsonConverter<Dictionary<string, FactModel>>
{
    public override Dictionary<string, FactModel> Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
    {
        if (reader.TokenType != JsonTokenType.StartObject)
        {
            throw new JsonException("Unexpected JsonTokenType");
        }

        var returnVar = new Dictionary<string, FactModel>();
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
            var value = JsonSerializer.Deserialize<FactModel>(ref reader, options); 

            returnVar.Add(propertyName, value);
        }

        return returnVar;
    }

    public override void Write(Utf8JsonWriter writer, Dictionary<string, FactModel> value, JsonSerializerOptions options)
    {
        throw new NotImplementedException();
    }
}