using Microsoft.Extensions.Logging;

namespace Sec.Edgar.CikProviders;

internal class CikEmptyProvider : CikBaseProvider
{
    internal CikEmptyProvider(
        ILogger? logger,
        int cikIdentifierLength, 
        bool fillCikIdentifierWithZeroes, 
        CancellationToken ctx) : base(logger, cikIdentifierLength, fillCikIdentifierWithZeroes, ctx)
    {
    }

    public override Task<string> GetFirstAsync(string identifier)
    {
        return Task.Run(() => CikDataManager.FillStringWithZeroes(identifier));
    }
    
    public override Task<string> GetFirstAsync(int cikNumber)
    {
        return GetFirstAsync(cikNumber.ToString());
    }
}