using System.Globalization;
using Sec.Edgar.CikProviders;
using Sec.Edgar.Enums;
using Sec.Edgar.Models.Edgar;

namespace Sec.Edgar.Models;

public class Submission
{
    private FileModel[] _filings = Array.Empty<FileModel>();
    
    internal Submission(SubmissionRoot rawModel, ICikProvider cikProvider)
    {
        CikProvider = cikProvider;
        CentralIndexKey = rawModel.CentralIndexKey;
        EntityType = rawModel.EntityType;
        StandardIndustrialClassification = rawModel.StandardIndustrialClassification;
        SicDescription = rawModel.SicDescription;
        InsiderTransactionForIssuerExists = rawModel.InsiderTransactionForIssuerExists == 1;
        InsiderTransactionForOwnerExists = rawModel.InsiderTransactionForOwnerExists == 1;
        CompanyName = rawModel.CompanyName;
        Tickers = rawModel.Tickers;
        Exchanges = rawModel.Exchanges;
        EmployerIdentificationNumber = rawModel.EmployerIdentificationNumber;
        Description = rawModel.Description;
        Category = rawModel.Category;
        StateOfIncorporation = rawModel.StateOfIncorporation;
        StateOfIncorporationDescription = rawModel.StateOfIncorporationDescription;
        FormerNames = rawModel.FormerNames;

        if (DateOnly.TryParseExact(rawModel.FiscalYearEnd, "MMdd", CultureInfo.InvariantCulture, DateTimeStyles.None,
                out var dt))
        {
            FiscalYearEnd = new DateOnly(DateTime.UtcNow.Year, dt.Month, dt.Day);
        }
        else
        {
            throw new FormatException($"Failed to convert {rawModel.FiscalYearEnd} to {nameof(DateOnly)}");
        }
    }
    public int CentralIndexKey { get; }
    public string EntityType { get; }
    public string StandardIndustrialClassification { get; }
    public string SicDescription { get; }
    public bool InsiderTransactionForOwnerExists { get; }
    public bool InsiderTransactionForIssuerExists { get; }
    public string CompanyName { get; }
    public string[] Tickers { get;  }
    public ExchangeType[] Exchanges { get;}
    public string EmployerIdentificationNumber { get; }
    public string Description { get; }
    public string Category { get;  }
    public DateOnly FiscalYearEnd { get; }
    public string StateOfIncorporation { get; }
    public string StateOfIncorporationDescription { get; }
    public FormerName[] FormerNames { get; }
    public FileModel[] Filings => _filings;

    internal readonly ICikProvider CikProvider;
    
    internal void AddFiles(FilingRecentModel? documentArray)
    {
        if (documentArray is null)
        {
            return;
        }
        
        var incomingDataLength = documentArray.AccessionNumber.Length;
        var newDataArray = new FileModel[incomingDataLength];
        for (var i = 0; i < incomingDataLength; i++)
        {
            newDataArray[i] = new FileModel(
                accessionNumber: documentArray.AccessionNumber[i],
                filingDate: documentArray.FilingDate[i],
                reportDate: documentArray.ReportDate[i],
                acceptanceDateTime: documentArray.AcceptanceDateTime[i],
                act: documentArray.Act[i],
                form: documentArray.Form[i],
                fileNumber: documentArray.FileNumber[i],
                filmNumber: documentArray.FilmNumber[i],
                items: documentArray.Items[i],
                size: documentArray.Size[i],
                isXBRL: documentArray.IsXBRL[i],
                isInlineXBRL: documentArray.IsInlineXBRL[i],
                primaryDocument: documentArray.PrimaryDocument[i],
                primaryDocDescription: documentArray.PrimaryDocDescription[i],
                submission: this);
        }

        var originalFilingSize = _filings.Length;
        Array.Resize(ref _filings, originalFilingSize + incomingDataLength);
        Array.Copy(newDataArray, 0, _filings, originalFilingSize, incomingDataLength);
    }
}