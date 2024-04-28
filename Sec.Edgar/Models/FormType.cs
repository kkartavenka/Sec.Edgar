namespace Sec.Edgar.Models;

public enum FormType
{
    Unrecognized,
    [FormEnum("10-K", ["10-K/A"])]
    Form10K,
    [FormEnum("10-Q", ["10-Q/A"])]
    Form10Q,
    [FormEnum("8-K", ["8-K/A"])]
    Form8K,
    [FormEnum("20-F")]
    Form20F,
    [FormEnum("11-K", ["11-K/A"])]
    Form11K,
    [FormEnum("40-F")]
    Form40F,
    [FormEnum("6-K")]
    Form6K,
    [FormEnum("SC 13D", ["SC 13D/A"])]
    Sc13D,
    [FormEnum("SC 13G", ["SC 13G/A"])]
    Sc13G,
    [FormEnum("ARS")]
    Ars,
    [FormEnum("DEF 14A", aliases: ["DEFA14A"])]
    Def14A,
    [FormEnum("4", ["4/A"])]
    Form4,
    [FormEnum("3", ["3/A"])]
    Form3,
    [FormEnum("5")]
    Form5,
    [FormEnum("S-8")]
    S8,
    [FormEnum("S-4", ["S-4/A"])]
    S4,
    [FormEnum("S-3", ["S-3/A"])]
    S3,
    [FormEnum("144")]
    Rule144,
    [FormEnum("EFFECT")]
    NotificationOfEffectiveness,
    [FormEnum("CORRESP")]
    Correspondence,
    [FormEnum("UPLOAD")]
    Upload,
    [FormEnum("424A")]
    Form424A,
    [FormEnum("424B5")]
    Form424B5,
    [FormEnum("424B1")]
    Form424B1,
    [FormEnum("424B3")]
    Form424B3,
    [FormEnum("424B4")]
    Form424B4,
    [FormEnum("424B7")]
    Form424B7,
    [FormEnum("S-8 POS")]
    FormS8POS,
    [FormEnum("SD")]
    FormSD,
    [FormEnum("S-3ASR")]
    FormS3ASR,
    [FormEnum("PRE 14A")]
    FormPre14A,
    [FormEnum("CERT")]
    Cert,
    [FormEnum("8-A12B")]
    Form8A12B,
    [FormEnum("FWP")]
    FreeWritingProspectuses,
    [FormEnum("NO ACT")]
    NoAction,
    [FormEnum("D")]
    FormD,
    [FormEnum("RW")]
    RegistrationWithdrawal
}