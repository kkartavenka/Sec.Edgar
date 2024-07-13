#if NET6_0_OR_GREATER
using System.Text.Json;
#elif NETSTANDARD2_0
using Newtonsoft.Json;
using Sec.Edgar.Extensions;
#endif
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Sec.Edgar.Models.Edgar;
using Sec.Edgar.Models.Exceptions;

namespace Sec.Edgar.CikProviders
{
    internal class CikJsonProvider : CikBaseProvider
    {
        private readonly string _absoluteSourceLocation;
        private readonly SourceType _sourceType;
        private readonly Uri _uri;
        private Func<Uri, CancellationToken, Task<Stream>> _getStreamHandler;

        public CikJsonProvider(
            ILogger logger,
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
            List<EdgarTickerJsonDto> dataset = null;
            switch (_sourceType)
            {
                case SourceType.Web:
                    dataset = await TryDeserializeFromWeb();
                    break;
                case SourceType.Local:
#if NET6_0_OR_GREATER
                    dataset = await TryDeserializeFromFile();
#elif NETSTANDARD2_0
                    dataset = TryDeserializeFromFile();
#endif
                    break;
            }

            if (dataset != null)
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

        public override async Task<List<EdgarTickerJsonDto>> GetAllAsync(int cikNumber)
        {
            Logger?.LogInformation($"{GetLogPrefix()}: invoked with {nameof(cikNumber)}: {cikNumber}");
            if (!CikDataManager.IsDataAvailable())
            {
                await UpdateCikDataset();
            }

            return CikDataManager.GetTickerInfo(cikNumber);
        }

        public override async Task<List<EdgarTickerJsonDto>> GetAllAsync(string identifier)
        {
            Logger?.LogInformation($"{GetLogPrefix()}: invoked with {nameof(identifier)}: {identifier}");
            if (!CikDataManager.IsDataAvailable())
            {
                await UpdateCikDataset();
            }

            return CikDataManager.GetTickerInfo(identifier);
        }

        private async Task<List<EdgarTickerJsonDto>> TryDeserializeFromWeb()
        {
            if (_getStreamHandler is null)
            {
                LogException(this,
                    new ExceptionEventArgs(new ArgumentNullException($"{nameof(_getStreamHandler)} is null"),
                        LogLevel.Critical, true));
                return null;
            }

            if (_uri is null)
            {
                LogException(this,
                    new ExceptionEventArgs(new ArgumentNullException($"{nameof(_uri)} is null"), LogLevel.Critical,
                        true));
                return null;
            }

            try
            {
                var stream = await _getStreamHandler(_uri, Ctx);
#if NET6_0_OR_GREATER
                return await TryDeserialize(stream);
#elif NETSTANDARD2_0
                return TryDeserialize(stream);
#endif
            }
            catch (Exception e)
            {
                LogException(this, new ExceptionEventArgs(e, LogLevel.Error));
                return null;
            }
        }

#if NET6_0_OR_GREATER
        private async Task<List<EdgarTickerJsonDto>> TryDeserializeFromFile()
#elif NETSTANDARD2_0
        private List<EdgarTickerJsonDto> TryDeserializeFromFile()
#endif
        {
            try
            {
                using (var fs = new FileStream(_absoluteSourceLocation, FileMode.Open))
                {
#if NET6_0_OR_GREATER
                    return await TryDeserialize(fs);
#elif NETSTANDARD2_0
                    return TryDeserialize(fs);
#endif
                }
            }
            catch (Exception e)
            {
                LogException(this, new ExceptionEventArgs(e, LogLevel.Error));
                return null;
            }
        }

#if NET6_0_OR_GREATER
        private async Task<List<EdgarTickerJsonDto>> TryDeserialize(Stream stream)
#elif NETSTANDARD2_0
        private List<EdgarTickerJsonDto> TryDeserialize(Stream stream)
#endif
        {
            try
            {
                var parsedResult =
#if NET6_0_OR_GREATER
                    await JsonSerializer.DeserializeAsync<Dictionary<string, EdgarTickerJsonDto>>(stream,
                        cancellationToken: Ctx);
#elif NETSTANDARD2_0
                    JsonConvert.DeserializeObject<Dictionary<string, EdgarTickerJsonDto>>(stream.GetString());
#endif

                if (parsedResult is null)
                {
                    LogException(this,
                        new ExceptionEventArgs(new Exception($"{_absoluteSourceLocation} contains no values"),
                            LogLevel.Warning));
                    return null;
                }

                return parsedResult.Select(x => x.Value)
                    .ToList();
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

        private static string GetLogPrefix([CallerMemberName] string caller = "")
        {
            return $"{nameof(CikJsonProvider)}::{caller}";
        }
    }
}