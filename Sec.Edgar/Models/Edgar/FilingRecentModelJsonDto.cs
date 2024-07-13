#if NET6_0_OR_GREATER
using System.Text.Json.Serialization;
#elif NETSTANDARD2_0
using Newtonsoft.Json;
#endif
using System;
using Sec.Edgar.Converters;
using Sec.Edgar.Enums;

namespace Sec.Edgar.Models.Edgar
{
    internal class FilingRecentModelJsonDto
    {
#if NET6_0_OR_GREATER
        [JsonPropertyName("accessionNumber")]
#elif NETSTANDARD2_0
        [JsonProperty("accessionNumber")]
#endif
        public string[] AccessionNumber { get; set; }

#if NET6_0_OR_GREATER
        [JsonPropertyName("filingDate")]
#elif NETSTANDARD2_0
        [JsonProperty("filingDate")]
#endif
        [JsonConverter(typeof(DateTimeArrayConverter))]
        public DateTime?[] FilingDate { get; set; }

#if NET6_0_OR_GREATER
        [JsonPropertyName("reportDate")]
#elif NETSTANDARD2_0
        [JsonProperty("reportDate")]
#endif
        [JsonConverter(typeof(DateTimeArrayConverter))]
        public DateTime?[] ReportDate { get; set; }

#if NET6_0_OR_GREATER
        [JsonPropertyName("acceptanceDateTime")]
#elif NETSTANDARD2_0
        [JsonProperty("acceptanceDateTime")]
#endif
        [JsonConverter(typeof(DateTimeArrayConverter))]
        public DateTime?[] AcceptanceDateTime { get; set; }

#if NET6_0_OR_GREATER
        [JsonPropertyName("act")]
#elif NETSTANDARD2_0
        [JsonProperty("act")]
#endif
        public string[] Act { get; set; }

#if NET6_0_OR_GREATER
        [JsonPropertyName("form")]
#elif NETSTANDARD2_0
        [JsonProperty("form")]
#endif
        [JsonConverter(typeof(FormTypeArrayConverter))]
        public FormType[] Form { get; set; }

#if NET6_0_OR_GREATER
        [JsonPropertyName("fileNumber")]
#elif NETSTANDARD2_0
        [JsonProperty("fileNumber")]
#endif
        public string[] FileNumber { get; set; }

#if NET6_0_OR_GREATER
        [JsonPropertyName("filmNumber")]
#elif NETSTANDARD2_0
        [JsonProperty("filmNumber")]
#endif
        public string[] FilmNumber { get; set; }

#if NET6_0_OR_GREATER
        [JsonPropertyName("items")]
#elif NETSTANDARD2_0
        [JsonProperty("items")]
#endif
        public string[] Items { get; set; }

#if NET6_0_OR_GREATER
        [JsonPropertyName("size")]
#elif NETSTANDARD2_0
        [JsonProperty("size")]
#endif
        public int[] Size { get; set; }

#if NET6_0_OR_GREATER
        [JsonPropertyName("isXBRL")]
#elif NETSTANDARD2_0
        [JsonProperty("isXBRL")]
#endif
        [JsonConverter(typeof(BoolArrayConverter))]
        public bool[] IsXBRL { get; set; }

#if NET6_0_OR_GREATER
        [JsonPropertyName("isInlineXBRL")]
#elif NETSTANDARD2_0
        [JsonProperty("isInlineXBRL")]
#endif
        [JsonConverter(typeof(BoolArrayConverter))]
        public bool[] IsInlineXBRL { get; set; }

#if NET6_0_OR_GREATER
        [JsonPropertyName("primaryDocument")]
#elif NETSTANDARD2_0
        [JsonProperty("primaryDocument")]
#endif
        public string[] PrimaryDocument { get; set; }

#if NET6_0_OR_GREATER
        [JsonPropertyName("primaryDocDescription")]
#elif NETSTANDARD2_0
        [JsonProperty("primaryDocDescription")]
#endif
        public string[] PrimaryDocDescription { get; set; }
    }
}