using Sec.Edgar.Models;

namespace Sec.Edgar.Enums;

public enum Taxonomy
{
    Unrecognized,
    [SpecialEnum("us-gaap")]
    USGaap, 
    [SpecialEnum("ifrs-full")]
    IfrsFull,
    [SpecialEnum("dei")]
    Dei, 
    [SpecialEnum("srt")]
    Srt,
    [SpecialEnum("invest")]
    Invest
}