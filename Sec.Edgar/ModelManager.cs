using System.Text;
using Sec.Edgar.Models;

namespace Sec.Edgar;

internal class ModelManager
{
    private readonly object _lock = new();
    private readonly int _cikIdentifierLength;
    private readonly bool _fillCikIdentifierWithZeroes;
    
    private Dictionary<int, EdgarTickerModel> _cikDict = new();
    private Dictionary<string, EdgarTickerModel> _tickerDict = new();

    internal ModelManager(int cikIdentifierLength, bool fillCikIdentifierWithZeroes)
    {
        _cikIdentifierLength = cikIdentifierLength;
        _fillCikIdentifierWithZeroes = fillCikIdentifierWithZeroes;
    }

    internal bool IsDataAvailable() => _cikDict.Any() && _tickerDict.Any();

    internal event EventHandler<ExceptionEventArgs>? ExceptionHandler;

    internal void LoadData(List<EdgarTickerModel> tickersInfo)
    {
        lock (_lock)
        {
            _cikDict = new Dictionary<int, EdgarTickerModel>(tickersInfo.Count);
            _tickerDict = new Dictionary<string, EdgarTickerModel>(tickersInfo.Count);
                    
            foreach (var tickerInfo in tickersInfo)
            {
                if (!_cikDict.TryAdd(tickerInfo.Cik, tickerInfo))
                {
                    var exceptionMessage =
                        $"The dictionary contains item with CIK {tickerInfo.Cik}. Existing ticket: {_cikDict[tickerInfo.Cik].Ticker}, trying adding: {tickerInfo.Ticker}";
                    ExceptionHandler?.Invoke(this, new ExceptionEventArgs(new Exception(exceptionMessage), false));
                }

                if (!_tickerDict.TryAdd(tickerInfo.Ticker.ToLower().Trim(), tickerInfo))
                {
                    var exceptionMessage =
                        $"The dictionary contains item with ticker {tickerInfo.Ticker}. Existing CIK: {_tickerDict[tickerInfo.Ticker].Cik}, trying adding: {tickerInfo.Cik}";
                    ExceptionHandler?.Invoke(this, new ExceptionEventArgs(new Exception(exceptionMessage), false));
                }
            }
        }
    }

    internal string GetCik(string identifier)
    {
        var tickerInfo = GetTickerInfo(identifier);
        return tickerInfo is null ? string.Empty : FillStringWithZeroes(tickerInfo.CikStr);
    }

    internal string GetCik(int identifier)
    {
        var tickerInfo = GetTickerInfo(identifier);
        return tickerInfo is null ? string.Empty : FillStringWithZeroes(tickerInfo.CikStr);
    }

    internal EdgarTickerModel? GetTickerInfo(int identifier)
    {
        var tickerInfo =_cikDict.GetValueOrDefault(identifier);
        if (tickerInfo is null)
        {
            ExceptionHandler?.Invoke(this,
                new ExceptionEventArgs(new Exception($"Cannot find CIK through provided identifier: {identifier}"),
                    false));
        }

        return tickerInfo;
    }

    internal EdgarTickerModel? GetTickerInfo(string identifier)
    {
        var cikNumber = TryGetNumericCik(identifier);

        if (cikNumber is not null)
        {
            return GetTickerInfo(cikNumber.Value);
        }

        var findByTicker = _tickerDict.GetValueOrDefault(identifier.ToLower().Trim());
        if (findByTicker is not null)
        {
            return findByTicker;
        }

        var findByName = _cikDict
            .Where(x => x.Value.Title.Contains(identifier, StringComparison.InvariantCultureIgnoreCase))
            .ToList();

        if (!findByName.Any())
        {
            ExceptionHandler?.Invoke(this,
                new ExceptionEventArgs(new Exception($"Cannot find CIK through provided identifier: {identifier}"),
                    false));
        }

        if (findByName.Count > 1)
        {
            ExceptionHandler?.Invoke(this,
                new ExceptionEventArgs(
                    new Exception($"Found more than one match for provided identifier: {identifier}"), false));
        }

        return findByName.First().Value;
    }
    
    private int? TryGetNumericCik(string identifier)
    {
        if (int.TryParse(identifier, out var returnVar))
        {
            return returnVar;
        }

        return null;
    }
    
    internal string FillStringWithZeroes(string identifier)
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

}