using System.ComponentModel.DataAnnotations;

namespace Sec.Edgar.Tests;

public class Tests
{
    [Test]
    public void ValidationFailure()
    {
        var builder = new ClientInfoBuilder();
        Assert.Throws<ArgumentException>(() => builder.WithUserAgent("Failed agent"));
        Assert.Throws<ArgumentException>(() => builder.WithRateLimit(0));
        Assert.Throws<ArgumentOutOfRangeException>(() => builder.WithCikIdentifierLength(1));
        Assert.Throws<Exception>(() => builder.Build());
    }

    [Test]
    public void IntendedUsage()
    {
        var clientInfo = new ClientInfoBuilder()
            .WithUserAgent("Company company@company.org")
            .Build();
        Assert.That(clientInfo, Is.Not.EqualTo(null));
    }
}