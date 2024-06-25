using Sec.Edgar.Enums;

namespace Sec.Edgar.Models;

public class Ticker
{
    public required string Name { get; init; }
    public required ExchangeType Exchange { get; init; }
}