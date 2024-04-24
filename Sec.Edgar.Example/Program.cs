using Sec.Edgar;
using Sec.Edgar.CikProviders;

var clientInfo = new ClientInfoBuilder()
    .WithCikProvider(CikProviderType.Json, EdgarConstants.CikJsonCompanyTicker)
    .WithUserAgent("My Demo Company mycompany@example.com")
    .WithCikIdentifierLength(10)
    .Build();
    
var client = new EdgarClient(clientInfo);
await client.GetSubmission("jwn");