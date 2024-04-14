using System.Collections.Concurrent;
using Sec.Edgar.Models;

namespace Sec.Edgar;

public interface ICikProvider
{
    public Task<string> GetAsync(string identifier);
    public Task<EdgarTickerModel> GetRecordAsync(int cikNumber);
    public Task<EdgarTickerModel> GetRecordAsync(string identifier);
    public ConcurrentDictionary<long, Exception> Exceptions { get; }
}