using System.Text.Json.Serialization;

namespace Sec.Edgar.Models.Edgar;

internal class FilesModel
{
    [JsonPropertyName("recent")] public required FilingRecentModel RecentFiles { get; init; }

    [JsonPropertyName("files")] public required FileModel[] Files { get; init; }
}