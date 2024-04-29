namespace Sec.Edgar.Models;

public enum Taxonomy
{
    Unrecognized,
    [FormEnum("us-gaap")]
    USGaap, 
    [FormEnum("ifrs-full")]
    IfrsFull,
    [FormEnum("dei")]
    Dei, 
    [FormEnum("srt")]
    Srt,
    [FormEnum("invest")]
    Invest
}