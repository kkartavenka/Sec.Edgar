using System.Text.Json.Serialization;

namespace Sec.Edgar.Models.Edgar;

public class FactModel
{
    [JsonPropertyName("label")]
    public required string Label { get; init; }
    
    [JsonPropertyName("description")]
    public required string Description { get; init; }
    
    [JsonPropertyName("units")]
    public required Dictionary<string, CompanyConceptRecord[]> Units { get; init; }
}