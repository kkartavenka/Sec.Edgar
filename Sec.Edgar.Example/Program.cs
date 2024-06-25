
using Microsoft.Extensions.Logging;
using Sec.Edgar;
using Sec.Edgar.Enums;
using Sec.Edgar.Example;

using var factory = LoggerFactory.Create(builder => builder.AddSimpleConsole(x =>
{
    x.SingleLine = true;
    x.UseUtcTimestamp = true;
}));

var clientInfo = new ClientInfoBuilder()
    .WithCikProvider(CikProviderType.Json, EdgarConstants.CikJsonCompanyTicker)
    .WithUserAgent("My Demo Company mycompany@example.com")
    .WithLoggerFactory(factory)
    .Build();

var client = new EdgarClient(clientInfo);

await new SubmissionsExample(client).Start();
await new CompanyConceptExample(client).Start();
await new CompanyFactExample(client).Start();
Console.Read();