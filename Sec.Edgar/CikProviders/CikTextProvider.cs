using System;
using System.IO;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;

namespace Sec.Edgar.CikProviders
{
    internal class CikTextProvider : CikBaseProvider
    {
        private readonly string _absoluteSourceLocation;
        private readonly SourceType _sourceType;
        private Func<Uri, CancellationToken, Task<Stream>> _getStreamHandler;

        internal CikTextProvider(
            Func<Uri, CancellationToken, Task<Stream>> getStreamHandler,
            ILogger logger,
            int cikIdentifierLength,
            bool fillCikIdentifierWithZeroes,
            string absoluteSourceLocation,
            CancellationToken ctx) : base(logger, cikIdentifierLength, fillCikIdentifierWithZeroes, ctx)
        {
            _getStreamHandler = getStreamHandler;
            _sourceType = GetSourceType(absoluteSourceLocation);
            _absoluteSourceLocation = absoluteSourceLocation;
        }
    }
}