# Sec.Edgar

## Preparing EDGAR client:

CIK, ticker and title information will be obtain anc cached from [SEC](https://www.sec.gov/files/company_tickers.json)
```csharp
var clientInfo = new ClientInfoBuilder()
    .WithUserAgent("My Demo Company mycompany@example.com")
    .WithCikProvider(CikProviderType.Json, EdgarConstants.CikJsonCompanyTicker)
    .Build();
```

In the case below, CIK number must be provided as `int` or `string` when using the client, otherwise Exception will be thrown:
```csharp
var clientInfo = new ClientInfoBuilder()
    .WithUserAgent("My Demo Company mycompany@example.com")
    .Build();
```
By default, a rate limit is applied, set by SEC to 10 requests per second. It can be overridden with `.WithRateLimit()`. SEC uses a CIK with a length of 10, if requirements are changed it can be overridden with `.WithCikIdentifierLength()`.

Getting the client:

```csharp
var client = new EdgarClient(clientInfo);
```
 
## Get submission (e.g. Northrop Grumman Corp):

```csharp
var submissionsByTicker = await client.GetAllSubmissions("noc");
var submissionsByName = await client.GetAllSubmissions("Northrop Grumman"); // By name or its part
var submissionsByCikStrFull = await client.GetAllSubmissions("0001133421");
var submissionsByCikStrShort = await client.GetAllSubmissions("1133421");
var submissionsByCik = await client.GetAllSubmissions(1133421);
```

Filings can be found inside `Filings` property of the `Submission`, it contains only partial information from SEC, specifically: `AccessionNumber`, `FilingDate`, `ReportDate`, `AcceptanceDateTime`, `Act`, `Form`, `FileNumber`, `FilmNumber` (Document Control Number), `Items`, `Size`, `IsXBRL`, `IsInlineXBRL`, `PrimaryDocument`, `PrimaryDocDescription`.

## Get `Uri` to the most recent 10-K report:

```csharp
var lastYearReport = submissions.Filings
    .Where(x => x.Form == FormType.Form10K)
    .MaxBy(x => x.FilingDate);

var link = await lastYearReport.GetLink();
```

## Get company concepts data for a company into a single API call

The following example demonstrate obtaining all `EntityCommonStockSharesOutstanding` expressed in `shares` units using Dei taxonomy:
```csharp
var company = await client.GetCompanyFacts("noc");
company.Facts[Taxonomy.Dei]["EntityCommonStockSharesOutstanding"].Units["shares"]
```

## Get company concepts data for XBRL tag

The following example demonstrate obtaining all `EntityCommonStockSharesOutstanding` expressed in `shares` units using Dei taxonomy:
```csharp
var concept = await client.GetCompanyConcept("noc", Taxonomy.Dei, "EntityCommonStockSharesOutstanding");
```
