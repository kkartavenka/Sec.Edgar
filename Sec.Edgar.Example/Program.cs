using Sec.Edgar;

var clientInfo = new ClientInfoBuilder()
    .WithCikProvider(CikProviderType.Json, EdgarConstants.CikJsonCompanyTicker)
    .WithUserAgent("My Demo Company mycompany@example.com")
    .WithCikIdentifierLength(10)
    .Build();
    
var client = new EdgarClient(clientInfo);
var facts = await client.GetCompanyFacts("noc");
var submissions = await client.GetAllSubmissions("noc");