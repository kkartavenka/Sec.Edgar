using System.Text.Json.Serialization;

namespace Sec.Edgar.Models.Edgar;

public class FilingRecentModel
{
    [JsonPropertyName("accessionNumber")]
    public string[] AccessionNumber { get; set; }
    
    [JsonPropertyName("filingDate")]
    public string[] FilingDate { get; set; }
    
    [JsonPropertyName("reportDate")]
    public string[] ReportDate { get; set; }
    
    [JsonPropertyName("acceptanceDateTime")]
    public string[] AcceptanceDateTime { get; set; }
    
    [JsonPropertyName("act")]
    public string[] Act { get; set; }
    
    [JsonPropertyName("form")]
    public string[] Form { get; set; }
    
    [JsonPropertyName("fileNumber")]
    public string[] FileNumber { get; set; }
    
    [JsonPropertyName("filmNumber")]
    public string[] FilmNumber { get; set; }
    
    [JsonPropertyName("items")]
    public string[] Items { get; set; }
    
    [JsonPropertyName("size")]
    public int[] Size { get; set; }
    
    [JsonPropertyName("isXBRL")]
    public int[] IsXBRL { get; set; }
    
    [JsonPropertyName("isInlineXBRL")]
    public int[] IsInlineXBRL { get; set; }
    
    [JsonPropertyName("primaryDocument")]
    public string[] PrimaryDocument { get; set; }
    
    [JsonPropertyName("primaryDocDescription")]
    public string[] PrimaryDocDescription { get; set; }
}