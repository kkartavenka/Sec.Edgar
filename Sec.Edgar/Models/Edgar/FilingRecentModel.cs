using System.Text.Json.Serialization;
using Sec.Edgar.Converters;
using Sec.Edgar.Enums;

namespace Sec.Edgar.Models.Edgar;

internal class FilingRecentModel
{
    [JsonPropertyName("accessionNumber")]
    public required string[] AccessionNumber { get; init; }
    
    [JsonPropertyName("filingDate"), JsonConverter(typeof(StringToDateArrayConverter))]
    public required DateTime?[] FilingDate { get; init; }
    
    [JsonPropertyName("reportDate"), JsonConverter(typeof(StringToDateArrayConverter))]
    public required DateTime?[] ReportDate { get; init; }
    
    [JsonPropertyName("acceptanceDateTime"), JsonConverter(typeof(StringToDateArrayConverter))]
    public required DateTime?[] AcceptanceDateTime { get; init; }
    
    [JsonPropertyName("act")]
    public required string[] Act { get; init; }
    
    [JsonPropertyName("form"), JsonConverter(typeof(StringToFormTypeArrayConverter))]
    public required FormType[] Form { get; init; }
    
    [JsonPropertyName("fileNumber")]
    public required string[] FileNumber { get; init; }
    
    [JsonPropertyName("filmNumber")]
    public required string[] FilmNumber { get; init; }
    
    [JsonPropertyName("items")]
    public required string[] Items { get; init; }
    
    [JsonPropertyName("size")]
    public required int[] Size { get; init; }
    
    [JsonPropertyName("isXBRL"), JsonConverter(typeof(StringToBoolArrayConverter))]
    public required bool[] IsXBRL { get; init; }
    
    [JsonPropertyName("isInlineXBRL"), JsonConverter(typeof(StringToBoolArrayConverter))]
    public required bool[] IsInlineXBRL { get; init; }
    
    [JsonPropertyName("primaryDocument")]
    public required string[] PrimaryDocument { get; init; }
    
    [JsonPropertyName("primaryDocDescription")]
    public required string[] PrimaryDocDescription { get; init; }
}