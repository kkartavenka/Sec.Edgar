using System.Text.Json.Serialization;

namespace Sec.Edgar.Models;

public class EdgarTickerModel
{
    public string CikStr => Cik.ToString();

    [JsonPropertyName("cik_str"), JsonRequired]
    public int Cik { get; init; }
    
    [JsonPropertyName("ticker"), JsonRequired]
    public required string Ticker { get; init; }
    
    [JsonPropertyName("title"), JsonRequired]
    public required string Title { get; init; }
}