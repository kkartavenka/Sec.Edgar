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
    internal class CompanyConceptRecordJsonDto
    {
#if NET6_0_OR_GREATER
        [JsonPropertyName("start")]
#elif NETSTANDARD2_0
        [JsonProperty("start")]
#endif
        [JsonConverter(typeof(DateTimeConverter))]
        public DateTime? StartDate { get; set; }

#if NET6_0_OR_GREATER
        [JsonPropertyName("end")]
#elif NETSTANDARD2_0
        [JsonProperty("end")]
#endif
        [JsonConverter(typeof(DateTimeConverter))]
        public DateTime? EndDate { get; set; }

#if NET6_0_OR_GREATER
        [JsonPropertyName("val")]
#elif NETSTANDARD2_0
        [JsonProperty("val")]
#endif
        [JsonConverter(typeof(NumericConverter<double>))]
        public double? Value { get; set; }

#if NET6_0_OR_GREATER
        [JsonPropertyName("accn")]
#elif NETSTANDARD2_0
        [JsonProperty("accn")]
#endif
        public string AccessionNumber { get; set; }

#if NET6_0_OR_GREATER
        [JsonPropertyName("fy")]
#elif NETSTANDARD2_0
        [JsonProperty("fy")]
#endif
        public int FiscalYear { get; set; }

#if NET6_0_OR_GREATER
        [JsonPropertyName("fp")]
#elif NETSTANDARD2_0
        [JsonProperty("fp")]
#endif
        [JsonConverter(typeof(EnumConverter<FiscalPeriod>))]
        public FiscalPeriod FiscalPeriod { get; set; }

#if NET6_0_OR_GREATER
        [JsonPropertyName("form")]
#elif NETSTANDARD2_0
        [JsonProperty("form")]
#endif
        [JsonConverter(typeof(FormTypeConverter))]
        public FormType Form { get; set; }

#if NET6_0_OR_GREATER
        [JsonPropertyName("filed")]
#elif NETSTANDARD2_0
        [JsonProperty("filed")]
#endif
        [JsonConverter(typeof(DateTimeConverter))]
        public DateTime? Filed { get; set; }

#if NET6_0_OR_GREATER
        [JsonPropertyName("frame")]
#elif NETSTANDARD2_0
        [JsonProperty("frame")]
#endif
        public string Frame { get; set; }
    }
}