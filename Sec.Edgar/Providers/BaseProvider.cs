using Sec.Edgar.CikProviders;

namespace Sec.Edgar.Providers;

internal class BaseProvider
{
    internal BaseProvider(ICikProvider cikProvider, CancellationToken ctx)
    {
        CikProvider = cikProvider;
        HttpClientWrapper = HttpClientWrapper.GetInitializedInstance();
        Ctx = ctx;
    }
    
    internal ICikProvider CikProvider { get; init; }
    internal HttpClientWrapper HttpClientWrapper { get; init; }
    internal CancellationToken Ctx { get; init; }
}