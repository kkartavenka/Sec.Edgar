#if NET6_0_OR_GREATER
using System.Text.Json.Serialization;
#elif NETSTANDARD2_0
using Newtonsoft.Json;
#endif
using System.Collections.Generic;

namespace Sec.Edgar.Models.Edgar
{
    internal class FactModelJsonDto
    {
#if NET6_0_OR_GREATER
        [JsonPropertyName("label")]
#elif NETSTANDARD2_0
        [JsonProperty("label")]
#endif
        public string Label { get; set; }

#if NET6_0_OR_GREATER
        [JsonPropertyName("description")]
#elif NETSTANDARD2_0
        [JsonProperty("description")]
#endif
        public string Description { get; set; }

#if NET6_0_OR_GREATER
        [JsonPropertyName("units")]
#elif NETSTANDARD2_0
        [JsonProperty("units")]
#endif
        public Dictionary<string, CompanyConceptRecordJsonDto[]> Units { get; set; }
    }
}