using System.Collections.Generic;
using Sec.Edgar.Converters;
using Sec.Edgar.Enums;
#if NET6_0_OR_GREATER
using System.Text.Json.Serialization;
#elif NETSTANDARD2_0
using Newtonsoft.Json;
#endif

namespace Sec.Edgar.Models.Edgar
{
    internal class CompanyFactJsonDto
    {
#if NET6_0_OR_GREATER
        [JsonPropertyName("cik")]
#elif NETSTANDARD2_0
#endif
        public int CentralIndexKey { get; set; }

#if NET6_0_OR_GREATER
        [JsonPropertyName("entityName")]
#elif NETSTANDARD2_0
    [JsonProperty("entityName")]
#endif
        public string EntityName { get; set; }

#if NET6_0_OR_GREATER
        [JsonPropertyName("facts")]
#elif NETSTANDARD2_0
    [JsonProperty("facts")]
#endif
        [JsonConverter(typeof(DictionaryEnumConverter))]
        public Dictionary<Taxonomy, Dictionary<string, FactModelJsonDto>> Facts { get; set; }
    }
}