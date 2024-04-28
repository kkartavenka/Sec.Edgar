using System.Reflection;
using System.Text.Json;
using System.Text.Json.Serialization;
using Sec.Edgar.Models;

namespace Sec.Edgar.Converters;

internal class StringToFormTypeArrayConverter : JsonConverter<FormType[]>
{
    private static List<KeyValuePair<string, string>> _formMapping = new();
    
    public override FormType[] Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
    {
        if (reader.TokenType != JsonTokenType.StartArray)
        {
            throw new JsonException("Unexpected JsonTokenType");
        }
        reader.Read();
        
        var elements = new List<FormType>();
        while (reader.TokenType != JsonTokenType.EndArray)
        {
            var value = reader.GetString();
            
            if (string.IsNullOrWhiteSpace(value))
            {
                elements.Add(FormType.Unrecognized);
            }
            else
            {
                var matchByAttribute = GetFormMapping()
                    .SingleOrDefault(x => x.Key.Equals(value, StringComparison.CurrentCultureIgnoreCase));

                elements.Add(Enum.TryParse(matchByAttribute.Value, out FormType observedType)
                    ? observedType
                    : FormType.Unrecognized);
            }

            reader.Read();
        }

        return elements.ToArray();
    }

    public override void Write(Utf8JsonWriter writer, FormType[] value, JsonSerializerOptions options)
    {
        throw new NotImplementedException();
    }

    private static List<KeyValuePair<string, string>> GetFormMapping()
    {
        if (_formMapping.Any())
        {
            return _formMapping;
        }
        
        _formMapping = typeof(FormType)
            .GetFields(BindingFlags.Public | BindingFlags.Static)
            .SelectMany(x =>
            {
                var feAttribute = x.GetCustomAttribute<FormEnumAttribute>();

                if (feAttribute is null)
                {
                    return Array.Empty<(string, string)>();
                }

                var aliasCount = feAttribute.Aliases?.Length ?? 0;
                var returnElements = new (string, string)[aliasCount + 1];
                returnElements[0] = (x.Name, feAttribute.Value.ToLower());

                if (feAttribute.Aliases is not null)
                {
                    var writeArrayPosition = 1;
                    foreach (var alias in feAttribute.Aliases)
                    {
                        returnElements[writeArrayPosition] = (x.Name, alias.ToLower());
                        writeArrayPosition++;
                    }
                }

                return returnElements;
            })
            .Select(x => new KeyValuePair<string, string>(x.Item2, x.Item1))
            .ToList();

        return _formMapping;
    }
}