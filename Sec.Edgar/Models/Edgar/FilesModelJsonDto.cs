#if NET6_0_OR_GREATER
using System.Text.Json.Serialization;
#elif NETSTANDARD2_0
using Newtonsoft.Json;
#endif

namespace Sec.Edgar.Models.Edgar
{
    internal class FilesModelJsonDto
    {
#if NET6_0_OR_GREATER
        [JsonPropertyName("recent")]
#elif NETSTANDARD2_0
        [JsonProperty("recent")]
#endif
        public FilingRecentModelJsonDto RecentFiles { get; set; }

#if NET6_0_OR_GREATER
        [JsonPropertyName("files")]
#elif NETSTANDARD2_0
        [JsonProperty("files")]
#endif
        public FileModelJsonDto[] Files { get; set; }
    }
}