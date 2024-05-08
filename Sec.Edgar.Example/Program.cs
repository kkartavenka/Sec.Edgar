using Sec.Edgar;
using Sec.Edgar.Enums;
using Sec.Edgar.Models;

var clientInfo = new ClientInfoBuilder()
    .WithCikProvider(CikProviderType.Json, EdgarConstants.CikJsonCompanyTicker)
    .WithUserAgent("My Demo Company mycompany@example.com")
    .Build();
    
var client = new EdgarClient(clientInfo);
var submissions = await client.GetAllSubmissions("0000895564");
if (submissions is not null)
{
    var lastYearReport = submissions.Filings
        .Where(x => x.Form == FormType.Form10K)
        .MaxBy(x => x.FilingDate);

    var link = await lastYearReport?.GetLink();
}

var company = await client.GetCompanyFacts("brk-b");

var concept = await client.GetCompanyConcept("brk-b", Taxonomy.Dei, "EntityCommonStockSharesOutstanding");
Console.Read();