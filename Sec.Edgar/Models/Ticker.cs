using Sec.Edgar.Enums;

namespace Sec.Edgar.Models
{
    public class Ticker
    {
        public Ticker(string name, ExchangeType exchangeType)
        {
            Name = name;
            Exchange = exchangeType;
        }

        public string Name { get; }
        public ExchangeType Exchange { get; }
    }
}