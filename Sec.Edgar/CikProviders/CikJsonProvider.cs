using System.Net;
using System.Runtime.Serialization;
using System.Text.Json;
using Sec.Edgar.Models;

namespace Sec.Edgar.CikProviders;

public class CikJsonProvider : CikBaseProvider
{
    private static HttpClient _httpClient;
    private readonly string _absoluteSourceLocation;
    private readonly SourceType _sourceType;
    private Dictionary<int, EdgarTickerModel> _cikModels = new();
    private readonly object _lock = new();

    public CikJsonProvider(HttpClient httpClient, int cikIdentifierLength, bool fillCikIdentifierWithZeroes, string absoluteSourceLocation, CancellationToken ctx) : base(cikIdentifierLength, fillCikIdentifierWithZeroes, ctx)
    {
        _httpClient = httpClient;
        _sourceType = GetSourceType(absoluteSourceLocation);
        _absoluteSourceLocation = absoluteSourceLocation;
    }

    public override async Task<string> GetAsync(string identifier)
    {
        return _sourceType switch
        {
            SourceType.None => await base.GetAsync(identifier),
            SourceType.Web => throw new NotImplementedException(),
            SourceType.Local => await ReadFromFile(identifier),
            _ => throw new ArgumentOutOfRangeException(nameof(_sourceType), _sourceType, "Unsupported CIK source type")
        };
    }

    public override async Task<EdgarTickerModel> GetRecordAsync(int cikNumber)
    {
        if (_sourceType == SourceType.None)
        {
            throw new Exception("To obtain the model, the Uri to ");
        }
        throw new NotImplementedException();
    }

    public override async Task<EdgarTickerModel> GetRecordAsync(string identifier)
    {
        throw new NotImplementedException();
    }

    private int? TryGetNumericCik(string identifier)
    {
        if (int.TryParse(identifier, out var returnVar))
        {
            return returnVar;
        }

        return null;
    }

    private async Task<string> ReadFromFile(string identifier)
    {
        if (!_cikModels.Any())
        {
            var tickersInfo = await TryDeserializeFromFile();

            if (tickersInfo is not null)
            {
                lock (_lock)
                {
                    _cikModels = new Dictionary<int, EdgarTickerModel>(tickersInfo!.Count);
                    foreach (var tickerInfo in tickersInfo)
                    {
                        if (_cikModels.TryAdd(tickerInfo.Cik, tickerInfo))
                        {
                            continue;
                        }

                        var exceptionMessage =
                            $"Duplication CIK {tickerInfo.Cik}. Dictionary contains: {_cikModels[tickerInfo.Cik].Ticker}, trying adding: {tickerInfo.Ticker}";
                        LogException(new Exception(exceptionMessage), false);
                    }
                }
            }
        }

        return FillStringWithZeroes(GetTickerInfo(identifier).CikStr);
    }

    private async Task<List<EdgarTickerModel>?> TryDeserializeFromFile()
    {
        try
        {
            await using var fs = new FileStream(_absoluteSourceLocation, FileMode.Open);
            return (await JsonSerializer.DeserializeAsync<Dictionary<string, EdgarTickerModel>>(fs, cancellationToken: Ctx))
                .Select(x => x.Value)
                .ToList();
        }
        catch (Exception e)
        {
            LogException(e, false);
            return null;
        }
    }

    private EdgarTickerModel GetTickerInfo(string identifier)
    {
        var cikNumber = TryGetNumericCik(identifier);

        if (cikNumber is not null)
        {
            return _cikModels[cikNumber.Value];
        }

        var cikModels = _cikModels
            .Where(x => x.Value.Ticker.Contains(identifier, StringComparison.InvariantCultureIgnoreCase))
            .ToList();

        if (!cikModels.Any())
        {
            LogException(new Exception($"Cannot find CIK through provided identifier: {identifier}"), false);
        }

        if (cikModels.Count > 1)
        {
            LogException(new Exception($"Found more than one match for provided identifier: {identifier}"), false);
        }

        return cikModels.First().Value;
    }

    private void LogException(Exception exception, bool throwException)
    {
        Exceptions.TryAdd(DateTimeOffset.UtcNow.ToUnixTimeMilliseconds(), exception);
        if (throwException)
        {
            throw exception;
        }
    }
}