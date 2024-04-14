using System.Text.Json.Serialization;

namespace Sec.Edgar.Models;

public class EdgarTickerModel
{
    public string CikStr => Cik.ToString();

    [JsonPropertyName("cik_str")]
    public int Cik { get; set; }
    
    [JsonPropertyName("ticker")]
    public string Ticker { get; set; }
    
    [JsonPropertyName("title")]
    public string Title { get; set; }
}