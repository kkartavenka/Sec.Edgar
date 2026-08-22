using System.Text.Json;
using Sec.Edgar.CikProviders;
using Sec.Edgar.Enums;
using Sec.Edgar.Models;
using Sec.Edgar.Models.Edgar;

namespace Sec.Edgar.Tests;

/// <summary>
///     Parses payloads shaped like the live EDGAR responses and checks the converters,
///     enum mappings and public projections.
/// </summary>
public class ParsingTests
{
    private static string Read(string file) =>
        File.ReadAllText(Path.Combine(TestContext.CurrentContext.TestDirectory, "Data", file));

    private static Submission BuildSubmission(string json)
    {
        var dto = JsonSerializer.Deserialize<SubmissionRootJsonDto>(json)!;
        var submission = new Submission(dto,
            new CikEmptyProvider(null, 10, true, CancellationToken.None));
        submission.AddFiles(dto.Files?.RecentFiles);
        return submission;
    }

    [Test]
    public void ParsesSubmissionHeaderFields()
    {
        var submission = BuildSubmission(Read("submission.json"));
        Assert.Multiple(() =>
        {
            // "cik" arrives as a JSON string on this endpoint.
            Assert.That(submission.CentralIndexKey, Is.EqualTo(1133421));
            Assert.That(submission.CompanyName, Is.EqualTo("NORTHROP GRUMMAN CORP"));
            Assert.That(submission.StandardIndustrialClassification, Is.EqualTo("3812"));
            Assert.That(submission.EmployerIdentificationNumber, Is.EqualTo("800640649"));
            Assert.That(submission.InsiderTransactionForIssuerExists, Is.True);
            Assert.That(submission.InsiderTransactionForOwnerExists, Is.False);
            Assert.That(submission.Tickers[0].Name, Is.EqualTo("NOC"));
            Assert.That(submission.Tickers[0].Exchange, Is.EqualTo(ExchangeType.NYSE));
            Assert.That(submission.FiscalYearEnd!.Value.ToString("MM-dd"), Is.EqualTo("12-31"));
            Assert.That(submission.FormerNames[0].Name, Is.EqualTo("NORTHROP GRUMMAN CORP /DE/"));
            Assert.That(submission.FormerNames[0].From.ToString("yyyy-MM-dd"), Is.EqualTo("2001-01-31"));
        });
    }

    [Test]
    public void ParsesFilingArraysColumnWise()
    {
        var filings = BuildSubmission(Read("submission.json")).Filings;
        Assert.Multiple(() =>
        {
            Assert.That(filings, Has.Length.EqualTo(3));
            Assert.That(filings[0].Form, Is.EqualTo(FormType.Form10K));
            Assert.That(filings[0].AccessionNumber, Is.EqualTo("0001133421-24-000010"));
            Assert.That(filings[0].FilingDate!.Value.ToString("yyyy-MM-dd"), Is.EqualTo("2024-01-25"));
            Assert.That(filings[0].IsXBRL, Is.True);
            Assert.That(filings[0].IsInlineXBRL, Is.True);
            Assert.That(filings[0].Size, Is.EqualTo(15370000));
            Assert.That(filings[1].Form, Is.EqualTo(FormType.Form10Q));
            Assert.That(filings[2].Form, Is.EqualTo(FormType.Form4));
            // An empty reportDate string must become null, not DateTime.MinValue.
            Assert.That(filings[2].ReportDate, Is.Null);
            Assert.That(filings[2].IsXBRL, Is.False);
        });
    }

    [Test]
    public async Task BuildsArchiveLinkForAFiling()
    {
        var filing = BuildSubmission(Read("submission.json")).Filings[0];
        var link = await filing.GetLink();
        Assert.That(link.AbsoluteUri, Is.EqualTo(
            "https://www.sec.gov/ix?doc=/Archives/edgar/data/0001133421/000113342124000010/noc-20231231.htm"));
    }

    [Test]
    public void ParsesCompanyConcept()
    {
        var dto = JsonSerializer.Deserialize<CompanyConceptJsonDto>(Read("concept.json"))!;
        var concept = new CompanyConcept(dto);
        var records = concept.Units["USD/shares"];
        Assert.Multiple(() =>
        {
            Assert.That(concept.CentralIndexKey, Is.EqualTo(1067983));
            Assert.That(concept.EntityName, Is.EqualTo("BERKSHIRE HATHAWAY INC"));
            Assert.That(concept.Taxonomy, Is.EqualTo(Taxonomy.USGaap));
            Assert.That(concept.Tag, Is.EqualTo("EarningsPerShareBasic"));
            Assert.That(records, Has.Length.EqualTo(2));
            Assert.That(records[0].Value, Is.EqualTo(8548d));
            Assert.That(records[0].FiscalPeriod, Is.EqualTo(FiscalPeriod.FiscalYear));
            Assert.That(records[0].Form, Is.EqualTo(FormType.Form10K));
            Assert.That(records[0].StartDate!.Value.ToString("yyyy-MM-dd"), Is.EqualTo("2007-01-01"));
            Assert.That(records[0].Frame, Is.EqualTo("CY2007"));
            Assert.That(records[1].Frame, Is.Null);
        });
    }

    [Test]
    public void ParsesCompanyFactsAcrossTaxonomies()
    {
        var dto = JsonSerializer.Deserialize<CompanyFactJsonDto>(Read("facts.json"))!;
        var facts = new CompanyFact(dto);
        var shares = facts.Facts[Taxonomy.Dei]["EntityCommonStockSharesOutstanding"].Units["shares"];
        Assert.Multiple(() =>
        {
            Assert.That(facts.CentralIndexKey, Is.EqualTo(19617));
            Assert.That(facts.EntityName, Is.EqualTo("JPMORGAN CHASE & CO"));
            Assert.That(facts.Facts.Keys, Is.EquivalentTo(new[]
            {
                Taxonomy.Dei, Taxonomy.USGaap, Taxonomy.Srt, Taxonomy.Invest
            }));
            Assert.That(shares[0].Value, Is.EqualTo(3900000000d));
            Assert.That(shares[0].FiscalPeriod, Is.EqualTo(FiscalPeriod.Q2));
        });
    }

    /// <summary>
    ///     Regression: two taxonomies the enum does not map both collapse onto
    ///     Taxonomy.Unrecognized and used to collide inside the dictionary.
    /// </summary>
    [Test]
    public void UnmappedTaxonomiesDoNotCollide()
    {
        const string json = """
        {"cik":1,"entityName":"X","facts":{
          "country":{"A":{"label":"l","description":"d","units":{"USD":[]}}},
          "us-gaap-sup":{"B":{"label":"l","description":"d","units":{"USD":[]}}}}}
        """;
        var dto = JsonSerializer.Deserialize<CompanyFactJsonDto>(json)!;
        Assert.That(new CompanyFact(dto).Facts, Contains.Key(Taxonomy.Unrecognized));
    }

    /// <summary>
    ///     Regression: these shapes all occur in live EDGAR data and each used to throw.
    /// </summary>
    [TestCase(@"{""cik"":""1"",""name"":""X"",""tickers"":[],""exchanges"":[],""formerNames"":[]}",
        TestName = "fiscalYearEnd absent")]
    [TestCase(@"{""cik"":""1"",""name"":""X"",""fiscalYearEnd"":"""",""tickers"":[],""exchanges"":[],""formerNames"":[]}",
        TestName = "fiscalYearEnd blank")]
    [TestCase(@"{""cik"":""1"",""name"":""X"",""fiscalYearEnd"":""0229"",""tickers"":[],""exchanges"":[],""formerNames"":[]}",
        TestName = "fiscalYearEnd on a leap day")]
    [TestCase(@"{""cik"":""1"",""name"":""X"",""fiscalYearEnd"":""1231"",""tickers"":[""ABC""],""exchanges"":[],""formerNames"":[]}",
        TestName = "more tickers than exchanges")]
    [TestCase(@"{""cik"":""1"",""name"":""X"",""fiscalYearEnd"":""1231"",""tickers"":[""ABC""],""exchanges"":[null],""formerNames"":[]}",
        TestName = "null exchange entry")]
    [TestCase(@"{""cik"":""1"",""name"":""X"",""fiscalYearEnd"":""1231"",""tickers"":[],""exchanges"":[]}",
        TestName = "formerNames absent")]
    public void SurvivesIncompleteSubmissionPayloads(string json)
    {
        Assert.DoesNotThrow(() => BuildSubmission(json));
    }

    [Test]
    public void FiscalYearEndIsNullWhenSecOmitsIt()
    {
        var submission = BuildSubmission(
            @"{""cik"":""1"",""name"":""X"",""tickers"":[],""exchanges"":[],""formerNames"":[]}");
        Assert.That(submission.FiscalYearEnd, Is.Null);
    }

    [Test]
    public void UnknownFormAndExchangeValuesDegradeGracefully()
    {
        var submission = BuildSubmission(
            @"{""cik"":""1"",""name"":""X"",""fiscalYearEnd"":""1231"",""tickers"":[""ABC""],
               ""exchanges"":[""SomeNewExchange""],""formerNames"":[],
               ""filings"":{""recent"":{""accessionNumber"":[""a""],""filingDate"":[""2024-01-01""],
               ""reportDate"":[""""],""acceptanceDateTime"":[""""],""act"":[""""],""form"":[""NOT-A-FORM""],
               ""fileNumber"":[""""],""filmNumber"":[""""],""items"":[""""],""size"":[1],""isXBRL"":[0],
               ""isInlineXBRL"":[0],""primaryDocument"":[""d.htm""],""primaryDocDescription"":[""""]},""files"":[]}}");
        Assert.Multiple(() =>
        {
            Assert.That(submission.Tickers[0].Exchange, Is.EqualTo(ExchangeType.Unknown));
            Assert.That(submission.Filings[0].Form, Is.EqualTo(FormType.Unrecognized));
        });
    }
}
