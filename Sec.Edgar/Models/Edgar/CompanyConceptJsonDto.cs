#if NET6_0_OR_GREATER
using System.Text.Json.Serialization;
#elif NETSTANDARD2_0
using Newtonsoft.Json;
#endif
using System.Collections.Generic;
using Sec.Edgar.Converters;
using Sec.Edgar.Enums;

namespace Sec.Edgar.Models.Edgar
{
    internal class CompanyConceptJsonDto
    {
#if NET6_0_OR_GREATER
        [JsonPropertyName("cik")]
#elif NETSTANDARD2_0
        [JsonProperty("cik")]
#endif
        public int CentralIndexKey { get; set; }

#if NET6_0_OR_GREATER
        [JsonPropertyName("taxonomy")]
#elif NETSTANDARD2_0
        [JsonProperty("taxonomy")]
#endif
        [JsonConverter(typeof(EnumConverter<Taxonomy>))]
        public Taxonomy Taxonomy { get; set; }

#if NET6_0_OR_GREATER
        [JsonPropertyName("tag")]
#elif NETSTANDARD2_0
        [JsonProperty("tag")]
#endif
        public string Tag { get; set; }

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
        [JsonPropertyName("entityName")]
#elif NETSTANDARD2_0
        [JsonProperty("entityName")]
#endif
        public string EntityName { get; set; }

#if NET6_0_OR_GREATER
        [JsonPropertyName("units")]
#elif NETSTANDARD2_0
        [JsonProperty("units")]
#endif
        public Dictionary<string, CompanyConceptRecordJsonDto[]> Units { get; set; }
    }
}