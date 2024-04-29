using System.Text.Json.Serialization;
using Sec.Edgar.Converters;

namespace Sec.Edgar.Models.Edgar;

public class CompanyConceptRecord
{
    [JsonPropertyName("start"), JsonConverter(typeof(StringToDateTimeConverter))]
    public DateTime? StartDate { get; init; }

    [JsonPropertyName("end"), JsonConverter(typeof(StringToDateTimeConverter))]
    public required DateTime? EndDate { get; init; }

    [JsonPropertyName("val"), JsonConverter(typeof(StringToNumericConverter<long>))]
    public long? Value { get; init; }

    [JsonPropertyName("accn")] public required string AccessionNumber { get; init; }

    [JsonPropertyName("fy")] public required int FiscalYear { get; init; }

    [JsonPropertyName("fp")] public required string FiscalPeriod { get; init; }

    [JsonPropertyName("form")] public required string Form { get; init; }

    [JsonPropertyName("filed"), JsonConverter(typeof(StringToDateTimeConverter))]
    public required DateTime? Filed { get; init; }

    [JsonPropertyName("frame")] public string? Frame { get; init; }
}