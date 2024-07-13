#if NET6_0_OR_GREATER
using System.Text.Json.Serialization;
#elif NETSTANDARD2_0
using Newtonsoft.Json;
#endif

namespace Sec.Edgar.Models.Edgar
{
    internal class FileModelJsonDto
    {
#if NET6_0_OR_GREATER
        [JsonPropertyName("name")]
#elif NETSTANDARD2_0
        [JsonProperty("name")]
#endif
        public string Name { get; set; }

#if NET6_0_OR_GREATER
        [JsonPropertyName("filingCount")]
#elif NETSTANDARD2_0
        [JsonProperty("filingCount")]
#endif
        public int FilingCount { get; set; }

#if NET6_0_OR_GREATER
        [JsonPropertyName("filingFrom")]
#elif NETSTANDARD2_0
        [JsonProperty("filingFrom")]
#endif
        public string FilingFrom { get; set; }

#if NET6_0_OR_GREATER
        [JsonPropertyName("filingTo")]
#elif NETSTANDARD2_0
        [JsonProperty("filingTo")]
#endif
        public string FilingTo { get; set; }
    }
}