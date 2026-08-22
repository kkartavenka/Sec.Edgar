using Sec.Edgar.Enums;

namespace Sec.Edgar.Example;

public class CompanyConceptExample(EdgarClient client)
{
    public async Task Start()
    {
        Console.WriteLine($"{Environment.NewLine}{nameof(CompanyConceptExample)}");
        
        var concept = await client.GetCompanyConcept("brk-b", Taxonomy.USGaap, "EarningsPerShareBasic");
        if (concept is null || concept.Units.Count == 0)
        {
            Console.WriteLine("No concept data returned");
            return;
        }

        Console.WriteLine($"Entity: {concept.EntityName}, CIK: {concept.CentralIndexKey}");
        Console.WriteLine($"Description: {concept.Description}");
        Console.WriteLine($"Tag of interest: {concept.Tag}, Taxonomy: {concept.Taxonomy}");

        var unit = concept.Units.First();
        Console.WriteLine($"Units: {unit.Key}");
        foreach (var record in unit.Value)
        {
            // Instantaneous concepts carry no "start", so neither date is guaranteed.
            var start = record.StartDate?.ToShortDateString() ?? "n/a";
            var end = record.EndDate?.ToShortDateString() ?? "n/a";
            Console.WriteLine($"{start}-{end}. Value: {record.Value}. Form {record.Form.ToString()}");
        }
    }
}