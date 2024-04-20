namespace Sec.Edgar.CikProviders;

public class CikEmptyProvider : CikBaseProvider
{
    public CikEmptyProvider(int cikIdentifierLength, bool fillCikIdentifierWithZeroes, CancellationToken ctx) : base(cikIdentifierLength, fillCikIdentifierWithZeroes, ctx)
    {
    }

    public override Task<string> GetAsync(string identifier)
    {
        return Task.Run(() => CikDataManager.FillStringWithZeroes(identifier));
    }
}