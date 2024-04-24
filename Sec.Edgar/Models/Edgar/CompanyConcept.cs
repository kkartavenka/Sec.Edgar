using System.Text.Json.Serialization;

namespace Sec.Edgar.Models.Edgar;

public class CompanyConcept
{
    [JsonPropertyName("cik")] public int CentralIndexKey { get; set; }
    [JsonPropertyName("taxonomy")] public string Taxonomy { get; set; }
    [JsonPropertyName("tag")] public string Tag { get; set; }
    [JsonPropertyName("label")] public string Label { get; set; }
    [JsonPropertyName("description")] public string Description { get; set; }
    [JsonPropertyName("entityName")] public string EntityName { get; set; }
}