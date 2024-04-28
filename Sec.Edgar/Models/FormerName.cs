using System.Text.Json.Serialization;
using Sec.Edgar.Converters;

namespace Sec.Edgar.Models;

public class FormerName
{
    [JsonPropertyName("name")]
    public required string Name { get; init; }
    
    [JsonPropertyName("from"), JsonConverter(typeof(StringToDateTimeConverter))]
    public required DateTime? From { get; init; }
    
    [JsonPropertyName("to"), JsonConverter(typeof(StringToDateTimeConverter))]
    public required DateTime? To { get; init; }
}