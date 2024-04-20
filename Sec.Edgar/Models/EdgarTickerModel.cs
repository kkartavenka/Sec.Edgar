using System.Text.Json.Serialization;

namespace Sec.Edgar.Models;

public class EdgarTickerModel
{
    public string CikStr => Cik.ToString();

    [JsonPropertyName("cik_str"), JsonRequired]
    public int Cik { get; set; }
    
    [JsonPropertyName("ticker"), JsonRequired]
    public string Ticker { get; set; }
    
    [JsonPropertyName("title"), JsonRequired]
    public string Title { get; set; }
}