namespace Sec.Edgar.Example;

public class CompanyFactExample(EdgarClient client)
{
    public async Task Start()
    {
        Console.WriteLine($"{Environment.NewLine}{nameof(CompanyFactExample)}");
        
        var company = await client.GetCompanyFacts("19617");
        Console.WriteLine($"Entity: {company?.EntityName}, CIK: {company?.CentralIndexKey}");
        Console.WriteLine($"Available facts taxonomies: {string.Join(", ", company.Facts.Select(x => $"{x.Key} [{x.Value.Count}]"))}");
    }
}