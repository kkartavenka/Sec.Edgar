namespace Sec.Edgar.CikProviders;

internal class CikEmptyProvider : CikBaseProvider
{
    internal CikEmptyProvider(int cikIdentifierLength, bool fillCikIdentifierWithZeroes, CancellationToken ctx) : base(cikIdentifierLength, fillCikIdentifierWithZeroes, ctx)
    {
    }

    public override Task<string> GetAsync(string identifier)
    {
        return Task.Run(() => CikDataManager.FillStringWithZeroes(identifier));
    }
    
    public override Task<string> GetAsync(int cikNumber)
    {
        return GetAsync(cikNumber.ToString());
    }
}