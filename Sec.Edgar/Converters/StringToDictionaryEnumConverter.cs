using System.Text.Json;
using System.Text.Json.Serialization;
using Sec.Edgar.Models;
using Sec.Edgar.Models.Edgar;

namespace Sec.Edgar.Converters;

public class StringToDictionaryEnumConverter : JsonConverter<Dictionary<Taxonomy, Dictionary<string, FactModel>>>
{
    private static List<KeyValuePair<string, string>> _formMapping = new();
    private readonly StringToDictionaryConverter _jsonConverter = new();
    
    public override Dictionary<Taxonomy, Dictionary<string, FactModel>> Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
    {
        if (reader.TokenType != JsonTokenType.StartObject)
        {
            throw new JsonException("Unexpected JsonTokenType");
        }

        var returnVar = new Dictionary<Taxonomy, Dictionary<string, FactModel>>();
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
            
            var matchByAttribute = GetFormMapping()
                .SingleOrDefault(x => x.Key.Equals(propertyName, StringComparison.CurrentCultureIgnoreCase));

            var parsedKey = Enum.TryParse(matchByAttribute.Value, out Taxonomy observedType)
                ? observedType
                : Taxonomy.Unrecognized;
            
            reader.Read();
            var value = _jsonConverter.Read(ref reader, null, options)!;
            
            returnVar.Add((Taxonomy)parsedKey, value);
        }

        return returnVar;
    }

    public override void Write(Utf8JsonWriter writer, Dictionary<Taxonomy, Dictionary<string, FactModel>> value, JsonSerializerOptions options)
    {
        throw new NotImplementedException();
    }
    
    private static List<KeyValuePair<string, string>> GetFormMapping()
    {
        if (_formMapping.Any())
        {
            return _formMapping;
        }

        _formMapping = EnumExtension.GetMapping<Taxonomy>();

        return _formMapping;
    }
}