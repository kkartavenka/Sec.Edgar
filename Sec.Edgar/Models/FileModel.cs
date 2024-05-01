namespace Sec.Edgar.Models;

public class FileModel
{
    internal FileModel(
        string accessionNumber, 
        DateTime? filingDate,
        DateTime? reportDate,
        DateTime? acceptanceDateTime,
        string act,
        FormType form,
        string fileNumber,
        string filmNumber,
        string items,
        int size,
        bool isXBRL,
        bool isInlineXBRL,
        string primaryDocument,
        string primaryDocDescription,
        Submission submission)
    {
        Submission = submission;
        AccessionNumber = accessionNumber;
        FilingDate = filingDate;
        ReportDate = reportDate;
        AcceptanceDateTime = acceptanceDateTime;
        Act = act;
        Form = form;
        FileNumber = fileNumber;
        FilmNumber = filmNumber;
        Items = items;
        Size = size;
        IsXBRL = isXBRL;
        IsInlineXBRL = isInlineXBRL;
        PrimaryDocument = primaryDocument;
        PrimaryDocDescription = primaryDocDescription;
    }
    
    public string AccessionNumber { get; }
    
    public DateTime? FilingDate { get; }
    
    public DateTime? ReportDate { get; }
    
    public DateTime? AcceptanceDateTime { get; }
    
    public string Act { get; }
    
    public FormType Form { get; }
    
    public string FileNumber { get; }
    
    public string FilmNumber { get; }
    
    public string Items { get; }
    
    public int Size { get; }
    
    public bool IsXBRL { get; }
    
    public bool IsInlineXBRL { get; }
    
    public string PrimaryDocument { get; }
    
    public string PrimaryDocDescription { get; }
    public Submission Submission { get; }

    public async Task<Uri> GetLink() =>
        new Uri(
            $"{EdgarConstants.ArchiveDocument}/{await Submission.CikProvider.GetAsync(Submission.CentralIndexKey)}/{AccessionNumber.Replace("-", "")}/{PrimaryDocument}");
}