using System.Text.Json;
using Sec.Edgar.Models;

namespace Sec.Edgar.CikProviders;

public class CikJsonProvider : CikBaseProvider
{
    private static HttpClient? _httpClient;
    private readonly string _absoluteSourceLocation;
    private readonly SourceType _sourceType;

    public CikJsonProvider(HttpClient? httpClient, int cikIdentifierLength, bool fillCikIdentifierWithZeroes, string absoluteSourceLocation, CancellationToken ctx) : base(cikIdentifierLength, fillCikIdentifierWithZeroes, ctx)
    {
        _httpClient = httpClient;
        _sourceType = GetSourceType(absoluteSourceLocation);
        _absoluteSourceLocation = absoluteSourceLocation;
    }

    public override async Task UpdateCikDataset()
    {
        var dataset = _sourceType switch
        {
            SourceType.Web => await TryDeserializeFromWeb(),
            SourceType.Local => await TryDeserializeFromFile(),
            _ => null
        };

        if (dataset is not null)
        {
            CikDataManager.LoadData(dataset);
        }
        else
        {
            LogException(new Exception("Failed to update"), false);
        }
    }

    public override async Task<string> GetAsync(string identifier)
    {
        if (!CikDataManager.IsDataAvailable())
        {
            await UpdateCikDataset();
        }

        return CikDataManager.GetCik(identifier);
    }
    
    public override async Task<string> GetAsync(int identifier)
    {
        if (!CikDataManager.IsDataAvailable())
        {
            await UpdateCikDataset();
        }

        return CikDataManager.GetCik(identifier);
    }

    public override async Task<EdgarTickerModel?> GetRecordAsync(int cikNumber)
    {
        if (!CikDataManager.IsDataAvailable())
        {
            await UpdateCikDataset();
        }

        return CikDataManager.GetTickerInfo(cikNumber);
    }

    public override async Task<EdgarTickerModel?> GetRecordAsync(string identifier)
    {
        if (!CikDataManager.IsDataAvailable())
        {
            await UpdateCikDataset();
        }

        return CikDataManager.GetTickerInfo(identifier);
    }

    private async Task<List<EdgarTickerModel>?> TryDeserializeFromWeb()
    {
        try
        {
            var stream = await _httpClient.GetStreamAsync(_absoluteSourceLocation, Ctx);
            return await TryDeserialize(stream);

        }
        catch (Exception e)
        {
            LogException(e, false);
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
            LogException(e, false);
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
                LogException(new Exception($"{_absoluteSourceLocation} contains no values"), false);
                return null;
            }

            return parsedResult.Select(x => x.Value).ToList();
        }
        catch (JsonException e)
        {
            LogException(e, false);
            return null;
        }
        catch (Exception e)
        {
            LogException(e, true);
            return null;
        }
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