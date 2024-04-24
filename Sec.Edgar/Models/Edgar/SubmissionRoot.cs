using System.Text.Json.Serialization;

namespace Sec.Edgar.Models.Edgar;

public class SubmissionRoot
{
    [JsonPropertyName("cik")] public string CentralIndexKey { get; set; }

    [JsonPropertyName("entityType")] public string EntityType { get; set; }

    [JsonPropertyName("sic")] public string StandardIndustrialClassification { get; set; }

    [JsonPropertyName("sicDescription")] public string SicDescription { get; set; }

    [JsonPropertyName("insiderTransactionForOwnerExists")]
    public int InsiderTransactionForOwnerExists { get; set; }

    [JsonPropertyName("insiderTransactionForIssuerExists")]
    public int InsiderTransactionForIssuerExists { get; set; }

    [JsonPropertyName("name")] public string CompanyName { get; set; }

    [JsonPropertyName("tickers")] public string[] Tickers { get; set; }

    [JsonPropertyName("exchanges")] public string[] Exchanges { get; set; }

    [JsonPropertyName("ein")] public string EmployerIdentificationNumber { get; set; }

    [JsonPropertyName("description")] public string Description { get; set; }

    [JsonPropertyName("website")] public string Website { get; set; }

    [JsonPropertyName("category")] public string Category { get; set; }

    [JsonPropertyName("fiscalYearEnd")] public string FiscalYearEnd { get; set; }

    [JsonPropertyName("stateOfIncorporation")]
    public string StateOfIncorporation { get; set; }

    [JsonPropertyName("stateOfIncorporationDescription")]
    public string StateOfIncorporationDescription { get; set; }

    [JsonPropertyName("formerNames")] public string[] FormerNames { get; set; }

    [JsonPropertyName("recent")] public FilingRecentModel RecentFiles { get; set; }

    [JsonPropertyName("files")] public FileModel[] Files { get; set; }
}