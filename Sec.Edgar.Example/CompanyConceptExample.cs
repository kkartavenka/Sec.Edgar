using Sec.Edgar.Enums;

namespace Sec.Edgar.Example;

public class CompanyConceptExample(EdgarClient client)
{
    public async Task Start()
    {
        Console.WriteLine($"{Environment.NewLine}{nameof(CompanyConceptExample)}");
        
        var concept = await client.GetCompanyConcept("brk-b", Taxonomy.USGaap, "EarningsPerShareBasic");
        Console.WriteLine($"Entity: {concept?.EntityName}, CIK: {concept?.CentralIndexKey}");
        Console.WriteLine($"Description: {concept?.Description}");
        Console.WriteLine($"Tag of interest: {concept?.Tag}, Taxonomy: {concept?.Taxonomy}");
        Console.WriteLine($"Units: {concept?.Units.First().Key}");
        foreach (var record in concept.Units.FirstOrDefault().Value)
        {
            Console.WriteLine($"{record.StartDate.Value.ToShortDateString()}-{record.EndDate.Value.ToShortDateString()}. Value: {record.Value}. Form {record.Form.ToString()}");
        }
    }
}