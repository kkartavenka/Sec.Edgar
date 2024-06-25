using Sec.Edgar.Enums;

namespace Sec.Edgar.Example;

public class SubmissionsExample(EdgarClient client)
{
    public async Task Start()
    {
        Console.WriteLine($"{Environment.NewLine}{nameof(SubmissionsExample)}");
        var submissions = await client.GetAllSubmissions("70858");
        
        if (submissions is not null)
        {
            Console.WriteLine(submissions.CompanyName);
            var lastYearReport = submissions.Filings
                .Where(x => x.Form == FormType.Form10K)
                .MaxBy(x => x.FilingDate);

            var link = await lastYearReport?.GetLink();
            Console.WriteLine($"Last year 10-K report link: {link}");
        }
    }
}