using System.Text.Json.Serialization;
using Sec.Edgar.Converters;
using Sec.Edgar.Enums;

namespace Sec.Edgar.Models.Edgar;

public class CompanyConceptRecord
{
    [JsonPropertyName("start"), JsonConverter(typeof(StringToDateTimeConverter))]
    public DateTime? StartDate { get; init; }

    [JsonPropertyName("end"), JsonConverter(typeof(StringToDateTimeConverter))]
    public required DateTime? EndDate { get; init; }

    [JsonPropertyName("val"), JsonConverter(typeof(StringToNumericConverter<double>))]
    public double? Value { get; init; }

    [JsonPropertyName("accn")] public required string AccessionNumber { get; init; }

    [JsonPropertyName("fy")] public required int FiscalYear { get; init; }

    [JsonPropertyName("fp"), JsonConverter(typeof(StringToEnumConverter<FiscalPeriod>))] public required FiscalPeriod FiscalPeriod { get; init; }

    [JsonPropertyName("form"), JsonConverter(typeof(StringToFormTypeConverter))] 
    public required FormType Form { get; init; }

    [JsonPropertyName("filed"), JsonConverter(typeof(StringToDateTimeConverter))]
    public required DateTime? Filed { get; init; }

    [JsonPropertyName("frame")] public string? Frame { get; init; }
}