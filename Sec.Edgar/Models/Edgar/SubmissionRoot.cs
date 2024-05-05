using System.Text.Json.Serialization;
using Sec.Edgar.Converters;
using Sec.Edgar.Enums;

namespace Sec.Edgar.Models.Edgar;

internal class SubmissionRoot
{
    [JsonPropertyName("cik"), JsonConverter(typeof(StringToNumericConverter<int>))] 
    public required int CentralIndexKey { get; init; }

    [JsonPropertyName("entityType")] public required string EntityType { get; init; }

    [JsonPropertyName("sic")] public required string StandardIndustrialClassification { get; init; }

    [JsonPropertyName("sicDescription")] public required string SicDescription { get; init; }

    [JsonPropertyName("insiderTransactionForOwnerExists")]
    public required int InsiderTransactionForOwnerExists { get; init; }

    [JsonPropertyName("insiderTransactionForIssuerExists")]
    public required int InsiderTransactionForIssuerExists { get; init; }

    [JsonPropertyName("name")] public required string CompanyName { get; init; }

    [JsonPropertyName("tickers")] public required string[] Tickers { get; init; }

    [JsonPropertyName("exchanges"), JsonConverter(typeof(StringToExchangeTypeArrayConverter))] 
    public required ExchangeType[] Exchanges { get; init; }

    [JsonPropertyName("ein")] public required string EmployerIdentificationNumber { get; init; }

    [JsonPropertyName("description")] public required string Description { get; init; }

    [JsonPropertyName("category")] public required string Category { get; init; }

    [JsonPropertyName("fiscalYearEnd")] public required string FiscalYearEnd { get; init; }

    [JsonPropertyName("stateOfIncorporation")]
    public required string StateOfIncorporation { get; init; }

    [JsonPropertyName("stateOfIncorporationDescription")]
    public required string StateOfIncorporationDescription { get; init; }

    [JsonPropertyName("formerNames")] public required FormerName[] FormerNames { get; init; }
    
    [JsonPropertyName("filings")] public required FilesModel Files { get; init; }
}