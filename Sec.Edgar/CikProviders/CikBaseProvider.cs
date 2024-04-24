using System.Collections.Concurrent;
using Sec.Edgar.Models;

namespace Sec.Edgar.CikProviders;

internal abstract class CikBaseProvider : ICikProvider
{
    protected CikBaseProvider(int cikIdentifierLength, bool fillCikIdentifierWithZeroes, CancellationToken ctx)
    {
        CikDataManager = new ModelManager(cikIdentifierLength, fillCikIdentifierWithZeroes);
        CikDataManager.ExceptionHandler += ModelManagerOnExceptionHandler;
        Ctx = ctx;
    }

    private void ModelManagerOnExceptionHandler(object? sender, ExceptionEventArgs e)
    {
        Console.WriteLine($"Exception: {sender}, e: {e.Exception}");
        Exceptions.TryAdd(DateTimeOffset.UtcNow.ToUnixTimeMilliseconds(), e.Exception);
        if (e.ReThrow)
        {
            throw e.Exception;
        }
    }

    public virtual Task<string> GetAsync(string identifier)
    {
        throw new NotImplementedException();
    }
    
    public virtual Task<string> GetAsync(int cikNumber)
    {
        throw new NotImplementedException();
    }

    public virtual Task<EdgarTickerModel?> GetRecordAsync(int cikNumber)
    {
        throw new NotImplementedException();
    }

    public virtual Task<EdgarTickerModel?> GetRecordAsync(string identifier)
    {
        throw new NotImplementedException();
    }

    public ConcurrentDictionary<long, Exception> Exceptions { get; } = new();
    public virtual Task UpdateCikDataset()
    {
        throw new NotImplementedException();
    }

    public CancellationToken Ctx { get; init; }
    
    internal readonly ModelManager CikDataManager;

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