using System.Collections.Concurrent;
using System.Text;
using Sec.Edgar.Models;

namespace Sec.Edgar.CikProviders;

public abstract class CikBaseProvider : ICikProvider
{
    private readonly int _cikIdentifierLength;
    private readonly bool _fillCikIdentifierWithZeroes;

    protected CikBaseProvider(int cikIdentifierLength, bool fillCikIdentifierWithZeroes, CancellationToken ctx)
    {
        Ctx = ctx;
        _cikIdentifierLength = cikIdentifierLength;
        _fillCikIdentifierWithZeroes = fillCikIdentifierWithZeroes;
    }

    public virtual Task<string> GetAsync(string identifier)
    {
        throw new NotImplementedException();
    }

    public virtual Task<EdgarTickerModel> GetRecordAsync(int cikNumber)
    {
        throw new NotImplementedException();
    }

    public virtual Task<EdgarTickerModel> GetRecordAsync(string identifier)
    {
        throw new NotImplementedException();
    }

    public ConcurrentDictionary<long, Exception> Exceptions { get; } = new();

    public CancellationToken Ctx { get; init; }

    protected string FillStringWithZeroes(string identifier)
    {
        if (!_fillCikIdentifierWithZeroes)
        {
            return identifier;
        }

        var zeroesCount = _cikIdentifierLength - identifier.Length;
        if (zeroesCount <= 0)
        {
            return identifier;
        }

        var sb = new StringBuilder(zeroesCount);
        for (var i = 0; i < zeroesCount; i++)
        {
            sb.Append('0');
        }

        sb.Append(identifier);

        return sb.ToString();
    }

    protected SourceType GetSourceType(string absoluteSourceLocation)
    {
        if (Uri.IsWellFormedUriString(absoluteSourceLocation, UriKind.Absolute))
        {
            return SourceType.Web;
        }

        if (File.Exists(absoluteSourceLocation))
        {
            return SourceType.Local;
        }

        return SourceType.None;
    }
}