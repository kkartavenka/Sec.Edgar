#if NET6_0_OR_GREATER
using System.Text.Json.Serialization;
#elif NETSTANDARD2_0
using Newtonsoft.Json;
#endif
using System;
using Sec.Edgar.Converters;

namespace Sec.Edgar.Models.Edgar
{
    internal class FormerNameJsonDto
    {
#if NET6_0_OR_GREATER
        [JsonPropertyName("name")]
#elif NETSTANDARD2_Ω
    [JsonProperty("name")]
#endif
        public string Name { get; set; }

        [JsonConverter(typeof(DateTimeConverter))]
#if NET6_0_OR_GREATER
        [JsonPropertyName("from")]
#elif NETSTANDARD2_0
        [JsonProperty("from")]
#endif
        public DateTime From { get; set; }

        [JsonConverter(typeof(DateTimeConverter))]
#if NET6_0_OR_GREATER
        [JsonPropertyName("to")]
#elif NETSTANDARD2_0
        [JsonProperty("to")]
#endif
        public DateTime To { get; set; }
    }
}