using System.Text.Json.Serialization;

namespace Sec.Edgar.Models.Edgar;

internal class CompanyConceptRecord
{
    [JsonPropertyName("end")] public required string EndDate { get; init; }

    [JsonPropertyName("val")] public required int Value { get; init; }

    [JsonPropertyName("accn")] public required string AccessionNumber { get; init; }

    [JsonPropertyName("fy")] public required int FiscalYear { get; init; }

    [JsonPropertyName("fp")] public required string FiscalPeriod { get; init; }

    [JsonPropertyName("form")] public required string Form { get; init; }

    [JsonPropertyName("filed")] public required string Filed { get; init; }

    [JsonPropertyName("frame")] public required string Frame { get; init; }
}