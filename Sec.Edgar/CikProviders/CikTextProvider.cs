using Microsoft.Extensions.Logging;

namespace Sec.Edgar.CikProviders;

internal class CikTextProvider : CikBaseProvider
{
    private Func<Uri, CancellationToken, Task<Stream>>? _getStreamHandler;
    private readonly string _absoluteSourceLocation;
    private readonly SourceType _sourceType;
    
    internal CikTextProvider(
        Func<Uri, CancellationToken, Task<Stream>>? getStreamHandler,
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