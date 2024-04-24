using System.Collections.Concurrent;
using Sec.Edgar.Models;

namespace Sec.Edgar;

internal interface ICikProvider
{
    internal Task<string> GetAsync(string identifier);
    internal Task<string> GetAsync(int cikNumber);
    internal Task<EdgarTickerModel?> GetRecordAsync(int cikNumber);
    internal Task<EdgarTickerModel?> GetRecordAsync(string identifier);
    internal ConcurrentDictionary<long, Exception> Exceptions { get; }
    internal Task UpdateCikDataset();
}