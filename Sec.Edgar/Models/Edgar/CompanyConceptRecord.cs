using System.Text.Json.Serialization;

namespace Sec.Edgar.Models.Edgar;

public class CompanyConceptRecord
{
    [JsonPropertyName("end")] public string EndDate { get; set; }

    [JsonPropertyName("val")] public int Value { get; set; }

    [JsonPropertyName("accn")] public string AccessionNumber { get; set; }

    [JsonPropertyName("fy")] public int FiscalYear { get; set; }

    [JsonPropertyName("fp")] public string FiscalPeriod { get; set; }

    [JsonPropertyName("form")] public string Form { get; set; }

    [JsonPropertyName("filed")] public string Filed { get; set; }

    [JsonPropertyName("frame")] public string Frame { get; set; }
}