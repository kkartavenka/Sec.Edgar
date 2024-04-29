using System.Text.Json.Serialization;
using Sec.Edgar.Converters;

namespace Sec.Edgar.Models.Edgar;

public class CompanyFact
{
    [JsonPropertyName("cik")] 
    public required int CentralIndexKey { get; init; }

    [JsonPropertyName("entityName")] public required string EntityName { get; init; }
    
    [JsonPropertyName("facts"), JsonConverter(typeof(StringToDictionaryEnumConverter))]
    public required Dictionary<Taxonomy, Dictionary<string, FactModel>> Facts {get; init; }
}