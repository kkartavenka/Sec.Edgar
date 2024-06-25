using Sec.Edgar.Models;
using Sec.Edgar.Models.Exceptions;

namespace Sec.Edgar.CikProviders;

internal interface ICikProvider
{
    internal Task<string> GetFirstAsync(string identifier);
    internal Task<string> GetFirstAsync(int cikNumber);
    internal Task<List<EdgarTickerModel>?> GetAllAsync(int cikNumber);
    internal Task<List<EdgarTickerModel>?> GetAllAsync(string identifier);
    internal Task UpdateCikDataset();
}