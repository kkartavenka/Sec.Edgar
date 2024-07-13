namespace Sec.Edgar.Enums
{
    public enum FormType
    {
        Unrecognized,

        [SpecialEnum("10-K", new[] { "10-K/A" })]
        Form10K,

        [SpecialEnum("10-Q", new[] { "10-Q/A" })]
        Form10Q,

        [SpecialEnum("8-K", new[] { "8-K/A" })]
        Form8K,

        [SpecialEnum("20-F")]
        Form20F,

        [SpecialEnum("11-K", new[] { "11-K/A" })]
        Form11K,

        [SpecialEnum("40-F")]
        Form40F,

        [SpecialEnum("6-K")]
        Form6K,

        [SpecialEnum("SC 13D", new[] { "SC 13D/A" })]
        Sc13D,

        [SpecialEnum("SC 13G", new[] { "SC 13G/A" })]
        Sc13G,

        [SpecialEnum("ARS")]
        Ars,

        [SpecialEnum("DEF 14A", new[] { "DEFA14A" })]
        Def14A,

        [SpecialEnum("4", new[] { "4/A" })]
        Form4,

        [SpecialEnum("3", new[] { "3/A" })]
        Form3,

        [SpecialEnum("5")]
        Form5,

        [SpecialEnum("S-8")]
        S8,

        [SpecialEnum("S-4", new[] { "S-4/A" })]
        S4,

        [SpecialEnum("S-3", new[] { "S-3/A" })]
        S3,

        [SpecialEnum("144")]
        Rule144,

        [SpecialEnum("EFFECT")]
        NotificationOfEffectiveness,

        [SpecialEnum("CORRESP")]
        Correspondence,

        [SpecialEnum("UPLOAD")]
        Upload,

        [SpecialEnum("424A")]
        Form424A,

        [SpecialEnum("424B5")]
        Form424B5,

        [SpecialEnum("424B1")]
        Form424B1,

        [SpecialEnum("424B3")]
        Form424B3,

        [SpecialEnum("424B4")]
        Form424B4,

        [SpecialEnum("424B7")]
        Form424B7,

        [SpecialEnum("S-8 POS")]
        FormS8POS,

        [SpecialEnum("SD")]
        FormSD,

        [SpecialEnum("S-3ASR")]
        FormS3ASR,

        [SpecialEnum("PRE 14A")]
        FormPre14A,

        [SpecialEnum("CERT")]
        Cert,

        [SpecialEnum("8-A12B")]
        Form8A12B,

        [SpecialEnum("FWP")]
        FreeWritingProspectuses,

        [SpecialEnum("NO ACT")]
        NoAction,

        [SpecialEnum("D")]
        FormD,

        [SpecialEnum("RW")]
        RegistrationWithdrawal
    }
}