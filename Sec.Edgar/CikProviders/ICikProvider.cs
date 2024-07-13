using System.Collections.Generic;
using System.Threading.Tasks;
using Sec.Edgar.Models.Edgar;

namespace Sec.Edgar.CikProviders
{
    internal interface ICikProvider
    {
        Task<string> GetFirstAsync(string identifier);
        Task<string> GetFirstAsync(int cikNumber);
        Task<List<EdgarTickerJsonDto>> GetAllAsync(int cikNumber);
        Task<List<EdgarTickerJsonDto>> GetAllAsync(string identifier);
        Task UpdateCikDataset();
    }
}