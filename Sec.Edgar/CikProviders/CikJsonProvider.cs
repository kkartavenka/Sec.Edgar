using System.Runtime.CompilerServices;
using System.Text.Json;
using Microsoft.Extensions.Logging;
using Sec.Edgar.Models;
using Sec.Edgar.Models.Exceptions;

namespace Sec.Edgar.CikProviders;

internal class CikJsonProvider : CikBaseProvider
{
    private Func<Uri, CancellationToken, Task<Stream>>? _getStreamHandler;
    private readonly string _absoluteSourceLocation;
    private readonly SourceType _sourceType;
    private readonly Uri? _uri;

    public CikJsonProvider(
        ILogger? logger,
        Func<Uri, CancellationToken, Task<Stream>> getStreamHandler,
        int cikIdentifierLength, bool fillCikIdentifierWithZeroes, 
        string absoluteSourceLocation, CancellationToken ctx) 
        : base(logger, cikIdentifierLength, fillCikIdentifierWithZeroes, ctx)
    {
        _getStreamHandler = getStreamHandler;
        _sourceType = GetSourceType(absoluteSourceLocation);
        if (_sourceType == SourceType.Web)
        {
            _uri = new Uri(absoluteSourceLocation);
        }
        _absoluteSourceLocation = absoluteSourceLocation;
    }

    public override async Task UpdateCikDataset()
    {
        Logger?.LogInformation($"{GetLogPrefix()}: invoked for {nameof(_sourceType)}: {_sourceType}");
        var dataset = _sourceType switch
        {
            SourceType.Web => await TryDeserializeFromWeb(),
            SourceType.Local => await TryDeserializeFromFile(),
            _ => null
        };

        if (dataset is not null)
        {
            CikDataManager.LoadData(dataset);
            Logger?.LogInformation($"{GetLogPrefix()}: completed");
        }
        else
        {
            LogException(this, new ExceptionEventArgs(new Exception("Failed to update"), LogLevel.Warning));
        }
    }

    public override async Task<string> GetFirstAsync(string identifier)
    {
        Logger?.LogInformation($"{GetLogPrefix()}: invoked with {nameof(identifier)}: {identifier}");
        if (!CikDataManager.IsDataAvailable())
        {
            await UpdateCikDataset();
        }

        return CikDataManager.GetCik(identifier);
    }
    
    public override async Task<string> GetFirstAsync(int identifier)
    {
        Logger?.LogInformation($"{GetLogPrefix()}: invoked with {nameof(identifier)}: {identifier}");
        if (!CikDataManager.IsDataAvailable())
        {
            await UpdateCikDataset();
        }

        return CikDataManager.GetCik(identifier);
    }

    public override async Task<List<EdgarTickerModel>?> GetAllAsync(int cikNumber)
    {
        Logger?.LogInformation($"{GetLogPrefix()}: invoked with {nameof(cikNumber)}: {cikNumber}");
        if (!CikDataManager.IsDataAvailable())
        {
            await UpdateCikDataset();
        }

        return CikDataManager.GetTickerInfo(cikNumber);
    }

    public override async Task<List<EdgarTickerModel>?> GetAllAsync(string identifier)
    {
        Logger?.LogInformation($"{GetLogPrefix()}: invoked with {nameof(identifier)}: {identifier}");
        if (!CikDataManager.IsDataAvailable())
        {
            await UpdateCikDataset();
        }

        return CikDataManager.GetTickerInfo(identifier);
    }

    private async Task<List<EdgarTickerModel>?> TryDeserializeFromWeb()
    {
        if (_getStreamHandler is null)
        {
            LogException(this,
                new ExceptionEventArgs(new ArgumentNullException($"{nameof(_getStreamHandler)} is null"), LogLevel.Critical , true));
            return null;
        }
        
        if (_uri is null)
        {
            LogException(this, new ExceptionEventArgs(new ArgumentNullException($"{nameof(_uri)} is null"), LogLevel.Critical, true));
            return null;
        }
        
        try
        {
            var stream = await _getStreamHandler(_uri, Ctx);
            return await TryDeserialize(stream);
        }
        catch (Exception e)
        {
            LogException(this, new ExceptionEventArgs(e, LogLevel.Error));
            return null;
        }
    }

    private async Task<List<EdgarTickerModel>?> TryDeserializeFromFile()
    {
        try
        {
            await using var fs = new FileStream(_absoluteSourceLocation, FileMode.Open);
            return await TryDeserialize(fs);
        }
        catch (Exception e)
        {
            LogException(this, new ExceptionEventArgs(e, LogLevel.Error));
            return null;
        }
    }

    private async Task<List<EdgarTickerModel>?> TryDeserialize(Stream stream)
    {
        try
        {
            var parsedResult =
                await JsonSerializer.DeserializeAsync<Dictionary<string, EdgarTickerModel>>(stream,
                    cancellationToken: Ctx);

            if (parsedResult is null)
            {
                LogException(this,
                    new ExceptionEventArgs(new Exception($"{_absoluteSourceLocation} contains no values"), LogLevel.Warning));
                return null;
            }

            return parsedResult.Select(x => x.Value).ToList();
        }
        catch (JsonException e)
        {
            LogException(this, new ExceptionEventArgs(e, LogLevel.Error));
            return null;
        }
        catch (Exception e)
        {
            LogException(this, new ExceptionEventArgs(e, LogLevel.Error, true));
            return null;
        }
    }

    private static string GetLogPrefix([CallerMemberName] string caller = "") => $"{nameof(CikJsonProvider)}::{caller}";
}