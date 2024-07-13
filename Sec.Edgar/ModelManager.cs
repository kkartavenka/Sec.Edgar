using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Sec.Edgar.Models.Edgar;
using Sec.Edgar.Models.Exceptions;

namespace Sec.Edgar
{
    internal class ModelManager
    {
        private readonly int _cikIdentifierLength;
        private readonly bool _fillCikIdentifierWithZeroes;
        private IReadOnlyList<EdgarTickerJsonDto> _tickersInfo;

        internal ModelManager(int cikIdentifierLength, bool fillCikIdentifierWithZeroes)
        {
            _cikIdentifierLength = cikIdentifierLength;
            _fillCikIdentifierWithZeroes = fillCikIdentifierWithZeroes;
        }

        internal bool IsDataAvailable()
        {
            return _tickersInfo != null && _tickersInfo.Any();
        }

        public event EventHandler<ExceptionEventArgs> ExceptionHandler;

        internal void LoadData(List<EdgarTickerJsonDto> tickersInfo)
        {
            var groupedTickerInfo = tickersInfo
                .GroupBy(x => (x.Cik, x.Ticker))
                .Select(x => (x.Count(), x.Key.Cik, x.Key.Ticker, x.ToList()))
                .ToList();

            groupedTickerInfo.Where(x => x.Item1 != 1)
                .ToList()
                .ForEach(x =>
                {
                    var exceptionMessage = $"The dictionary contains item with CIK {x.Cik} and ticker {x.Ticker}";
                    ExceptionHandler?.Invoke(this,
                        new ExceptionEventArgs(new CikDuplicateException(exceptionMessage), false));
                });

            var uniqueTickerInfo = new List<EdgarTickerJsonDto>(groupedTickerInfo.Count);
            groupedTickerInfo.ForEach(x => uniqueTickerInfo.Add(x.Item4.First()));

            _tickersInfo = uniqueTickerInfo;
        }

        internal string GetCik(string identifier)
        {
            var tickerInfo = GetTickerInfo(identifier);
            return tickerInfo is null
                ? string.Empty
                : FillStringWithZeroes(tickerInfo.First()
                    .CikStr);
        }

        internal string GetCik(int identifier)
        {
            var tickerInfo = GetTickerInfo(identifier);
            return tickerInfo is null
                ? string.Empty
                : FillStringWithZeroes(tickerInfo.First()
                    .CikStr);
        }

        internal List<EdgarTickerJsonDto> GetTickerInfo(int identifier)
        {
            if (!IsDataAvailable())
            {
                ExceptionHandler?.Invoke(this,
                    new ExceptionEventArgs(new Exception("CIK data is not available/loaded"), true));
            }

            var selectedItems = _tickersInfo?.Where(x => x.Cik == identifier)
                .ToList();
            if (selectedItems is null)
            {
                ExceptionHandler?.Invoke(this,
                    new ExceptionEventArgs(new Exception($"Cannot find CIK through provided identifier: {identifier}"),
                        false));
            }

            return selectedItems;
        }

        internal List<EdgarTickerJsonDto> GetTickerInfo(string identifier)
        {
            var cikNumber = TryGetNumericCik(identifier);

            if (cikNumber != null)
            {
                return GetTickerInfo(cikNumber.Value);
            }

            var findByTicker = _tickersInfo?
                .Where(x => string.Equals(x.Ticker, identifier.Trim(), StringComparison.InvariantCultureIgnoreCase))
                .ToList();

            if (findByTicker != null)
            {
                return findByTicker;
            }

            var findByName = _tickersInfo?
                .Where(x => x.Title.IndexOf(identifier, StringComparison.InvariantCultureIgnoreCase) >= 0)
                .ToList();

            if (findByName is null || findByName.Count == 0)
            {
                ExceptionHandler?.Invoke(this,
                    new ExceptionEventArgs(new Exception($"Cannot find CIK through provided identifier: {identifier}"),
                        false));
            }

            return findByName;
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
}