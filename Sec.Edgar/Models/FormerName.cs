using System;

namespace Sec.Edgar.Models
{
    public class FormerName
    {
        public FormerName(string name, DateTime from, DateTime to)
        {
            Name = name;
            From = from;
            To = to;
        }

        public string Name { get; }
        public DateTime From { get; }
        public DateTime To { get; }
    }
}