#if NET6_0_OR_GREATER
using System.Text.Json.Serialization;
#elif NETSTANDARD2_0
using Newtonsoft.Json;
#endif

namespace Sec.Edgar.Models.Edgar
{
    internal class EdgarTickerJsonDto
    {
        public string CikStr => Cik.ToString();

        [JsonRequired]
#if NET6_0_OR_GREATER
        [JsonPropertyName("cik_str")]
#elif NETSTANDARD2_0
        [JsonProperty("cik_str")]
#endif
        public int Cik { get; set; }

        [JsonRequired]
#if NET6_0_OR_GREATER
        [JsonPropertyName("ticker")]
#elif NETSTANDARD2_0
        [JsonProperty("ticker")]
#endif
        public string Ticker { get; set; }

        [JsonRequired]
#if NET6_0_OR_GREATER
        [JsonPropertyName("title")]
#elif NETSTANDARD2_0
        [JsonProperty("title")]
#endif
        public string Title { get; set; }
    }
}