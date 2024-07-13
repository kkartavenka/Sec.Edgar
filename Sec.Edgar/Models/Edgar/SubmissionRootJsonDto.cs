#if NET6_0_OR_GREATER
using System.Text.Json.Serialization;
#elif NETSTANDARD2_0
using Newtonsoft.Json;
#endif
using Sec.Edgar.Converters;
using Sec.Edgar.Enums;

namespace Sec.Edgar.Models.Edgar
{
    internal class SubmissionRootJsonDto
    {
#if NET6_0_OR_GREATER
        [JsonPropertyName("cik")]
#elif NETSTANDARD2_0
        [JsonProperty("cik")]
#endif
        [JsonConverter(typeof(NumericConverter<int>))]
        public int CentralIndexKey { get; set; }

#if NET6_0_OR_GREATER
        [JsonPropertyName("entityType")]
#elif NETSTANDARD2_0
        [JsonProperty("entityType")]
#endif
        public string EntityType { get; set; }

#if NET6_0_OR_GREATER
        [JsonPropertyName("sic")]
#elif NETSTANDARD2_0
        [JsonProperty("sic")]
#endif
        public string StandardIndustrialClassification { get; set; }

#if NET6_0_OR_GREATER
        [JsonPropertyName("sicDescription")]
#elif NETSTANDARD2_0
        [JsonProperty("sicDescription")]
#endif
        public string SicDescription { get; set; }

#if NET6_0_OR_GREATER
        [JsonPropertyName("insiderTransactionForOwnerExists")]
#elif NETSTANDARD2_0
        [JsonProperty("insiderTransactionForOwnerExists")]
#endif
        public int InsiderTransactionForOwnerExists { get; set; }

#if NET6_0_OR_GREATER
        [JsonPropertyName("insiderTransactionForIssuerExists")]
#elif NETSTANDARD2_0
        [JsonProperty("insiderTransactionForIssuerExists")]
#endif
        public int InsiderTransactionForIssuerExists { get; set; }

#if NET6_0_OR_GREATER
        [JsonPropertyName("name")]
#elif NETSTANDARD2_0
        [JsonProperty("name")]
#endif
        public string CompanyName { get; set; }

#if NET6_0_OR_GREATER
        [JsonPropertyName("tickers")]
#elif NETSTANDARD2_0
        [JsonProperty("tickers")]
#endif
        public string[] Tickers { get; set; }

#if NET6_0_OR_GREATER
        [JsonPropertyName("exchanges")]
#elif NETSTANDARD2_0
        [JsonProperty("exchanges")]
#endif
        [JsonConverter(typeof(ExchangeTypeArrayConverter))]
        public ExchangeType[] Exchanges { get; set; }

#if NET6_0_OR_GREATER
        [JsonPropertyName("ein")]
#elif NETSTANDARD2_0
        [JsonProperty("ein")]
#endif
        public string EmployerIdentificationNumber { get; set; }

#if NET6_0_OR_GREATER
        [JsonPropertyName("description")]
#elif NETSTANDARD2_0
        [JsonProperty("description")]
#endif
        public string Description { get; set; }

#if NET6_0_OR_GREATER
        [JsonPropertyName("category")]
#elif NETSTANDARD2_0
        [JsonProperty("category")]
#endif
        public string Category { get; set; }

#if NET6_0_OR_GREATER
        [JsonPropertyName("fiscalYearEnd")]
#elif NETSTANDARD2_0
        [JsonProperty("fiscalYearEnd")]
#endif
        public string FiscalYearEnd { get; set; }

#if NET6_0_OR_GREATER
        [JsonPropertyName("stateOfIncorporation")]
#elif NETSTANDARD2_0
        [JsonProperty("stateOfIncorporation")]
#endif
        public string StateOfIncorporation { get; set; }

#if NET6_0_OR_GREATER
        [JsonPropertyName("stateOfIncorporationDescription")]
#elif NETSTANDARD2_0
        [JsonProperty("stateOfIncorporationDescription")]
#endif
        public string StateOfIncorporationDescription { get; set; }

#if NET6_0_OR_GREATER
        [JsonPropertyName("formerNames")]
#elif NETSTANDARD2_0
        [JsonProperty("formerNames")]
#endif
        public FormerNameJsonDto[] FormerNames { get; set; }

#if NET6_0_OR_GREATER
        [JsonPropertyName("filings")]
#elif NETSTANDARD2_0
        [JsonProperty("filings")]
#endif
        public FilesModelJsonDto Files { get; set; }
    }
}