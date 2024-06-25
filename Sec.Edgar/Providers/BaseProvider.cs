using Microsoft.Extensions.Logging;
using Sec.Edgar.CikProviders;

namespace Sec.Edgar.Providers;

internal class BaseProvider
{
    internal BaseProvider(ICikProvider cikProvider, ILogger? logger, CancellationToken ctx)
    {
        CikProvider = cikProvider;
        HttpClientWrapper = HttpClientWrapper.GetInitializedInstance();
        Ctx = ctx;
        Logger = logger;
    }

    internal ILogger? Logger { get; private set; }
    internal ICikProvider CikProvider { get; init; }
    internal HttpClientWrapper HttpClientWrapper { get; init; }
    internal CancellationToken Ctx { get; init; }
}