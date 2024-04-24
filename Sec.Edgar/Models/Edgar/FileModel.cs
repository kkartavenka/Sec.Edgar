using System.Text.Json.Serialization;

namespace Sec.Edgar.Models.Edgar;

public class FileModel
{
    [JsonPropertyName("name")] public string Name { get; set; }

    [JsonPropertyName("filingCount")] public int FilingCount { get; set; }

    [JsonPropertyName("filingFrom")] public string FilingFrom { get; set; }

    [JsonPropertyName("filingTo")] public string FilingTo { get; set; }
}