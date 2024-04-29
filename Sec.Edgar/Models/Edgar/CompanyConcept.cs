using System.Text.Json.Serialization;

namespace Sec.Edgar.Models.Edgar;

public class CompanyConcept
{
    [JsonPropertyName("cik")] public required int CentralIndexKey { get; init; }
    [JsonPropertyName("taxonomy")] public required string Taxonomy { get; init; }
    [JsonPropertyName("tag")] public required string Tag { get; init; }
    [JsonPropertyName("label")] public required string Label { get; init; }
    [JsonPropertyName("description")] public required string Description { get; init; }
    [JsonPropertyName("entityName")] public required string EntityName { get; init; }
}