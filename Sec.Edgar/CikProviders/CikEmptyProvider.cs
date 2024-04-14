namespace Sec.Edgar.CikProviders;

public class CikEmptyProvider : CikBaseProvider
{
    public CikEmptyProvider(int cikIdentifierLength, bool fillCikIdentifierWithZeroes, CancellationToken ctx) : base(cikIdentifierLength, fillCikIdentifierWithZeroes, ctx)
    {
    }

    public async Task<string> Get(string identifier)
    {
        return FillStringWithZeroes(identifier);
    }
}