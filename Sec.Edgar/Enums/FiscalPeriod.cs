namespace Sec.Edgar.Enums
{
    public enum FiscalPeriod
    {
        Unrecognized,

        [SpecialEnum("q1")]
        Q1,

        [SpecialEnum("q2")]
        Q2,

        [SpecialEnum("q3")]
        Q3,

        [SpecialEnum("q4")]
        Q4,

        [SpecialEnum("fy")]
        FiscalYear
    }
}