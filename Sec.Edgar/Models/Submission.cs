using System;
using System.Collections.Generic;
using System.Globalization;
using Sec.Edgar.CikProviders;
using Sec.Edgar.Enums;
using Sec.Edgar.Models.Edgar;

namespace Sec.Edgar.Models
{
    public class Submission
    {
        internal readonly ICikProvider CikProvider;
        private FileModel[] _filings = Array.Empty<FileModel>();

        internal Submission(SubmissionRootJsonDto rawModel, ICikProvider cikProvider)
        {
            CikProvider = cikProvider;
            CentralIndexKey = rawModel.CentralIndexKey;
            EntityType = rawModel.EntityType;
            StandardIndustrialClassification = rawModel.StandardIndustrialClassification;
            SicDescription = rawModel.SicDescription;
            InsiderTransactionForIssuerExists = rawModel.InsiderTransactionForIssuerExists == 1;
            InsiderTransactionForOwnerExists = rawModel.InsiderTransactionForOwnerExists == 1;
            CompanyName = rawModel.CompanyName;
            EmployerIdentificationNumber = rawModel.EmployerIdentificationNumber;
            Description = rawModel.Description;
            Category = rawModel.Category;
            StateOfIncorporation = rawModel.StateOfIncorporation;
            StateOfIncorporationDescription = rawModel.StateOfIncorporationDescription;
            FormerNames = ConvertToPublic(rawModel.FormerNames);
            InitTickersArray(rawModel.Tickers, rawModel.Exchanges);
            FiscalYearEnd = ParseFiscalYearEnd(rawModel.FiscalYearEnd);
        }

        public int CentralIndexKey { get; }
        public string EntityType { get; }
        public string StandardIndustrialClassification { get; }
        public string SicDescription { get; }
        public bool InsiderTransactionForOwnerExists { get; }
        public bool InsiderTransactionForIssuerExists { get; }
        public string CompanyName { get; }
        public Ticker[] Tickers { get; private set; }
        public string EmployerIdentificationNumber { get; }
        public string Description { get; }
        public string Category { get; }
        public DateTime? FiscalYearEnd { get; }
        public string StateOfIncorporation { get; }
        public string StateOfIncorporationDescription { get; }
        public FormerName[] FormerNames { get; }
        public FileModel[] Filings => _filings;

        internal void AddFiles(FilingRecentModelJsonDto documentArray)
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
                    documentArray.AccessionNumber[i],
                    documentArray.FilingDate[i],
                    documentArray.ReportDate[i],
                    documentArray.AcceptanceDateTime[i],
                    documentArray.Act[i],
                    documentArray.Form[i],
                    documentArray.FileNumber[i],
                    documentArray.FilmNumber[i],
                    documentArray.Items[i],
                    documentArray.Size[i],
                    documentArray.IsXBRL[i],
                    documentArray.IsInlineXBRL[i],
                    documentArray.PrimaryDocument[i],
                    documentArray.PrimaryDocDescription[i],
                    this);
            }

            var originalFilingSize = _filings.Length;
            Array.Resize(ref _filings, originalFilingSize + incomingDataLength);
            Array.Copy(newDataArray, 0, _filings, originalFilingSize, incomingDataLength);
        }

        /// <summary>
        ///     SEC reports the fiscal year end as an "MMdd" string, but omits it (or leaves it blank)
        ///     for filers that do not have one, such as individuals filing Form 4. A value of "0229"
        ///     is also unrepresentable in a non-leap year. Neither case is an error, so report the
        ///     absence as null instead of throwing.
        /// </summary>
        private static DateTime? ParseFiscalYearEnd(string fiscalYearEnd)
        {
            if (!DateTime.TryParseExact(fiscalYearEnd, "MMdd", CultureInfo.InvariantCulture, DateTimeStyles.None,
                    out var dt))
            {
                return null;
            }

            var year = DateTime.UtcNow.Year;
            if (dt.Month == 2 && dt.Day == 29 && !DateTime.IsLeapYear(year))
            {
                return new DateTime(year, 2, 28);
            }

            return new DateTime(year, dt.Month, dt.Day);
        }

        private void InitTickersArray(IReadOnlyList<string> tickers, IReadOnlyList<ExchangeType> exchanges)
        {
            if (tickers is null)
            {
                Tickers = Array.Empty<Ticker>();
                return;
            }

            Tickers = new Ticker[tickers.Count];
            for (var i = 0; i < tickers.Count; i++)
            {
                // SEC does not guarantee that "exchanges" is as long as "tickers".
                var exchange = exchanges != null && i < exchanges.Count
                    ? exchanges[i]
                    : ExchangeType.Unknown;
                Tickers[i] = new Ticker(tickers[i], exchange);
            }
        }

        private static FormerName[] ConvertToPublic(IReadOnlyList<FormerNameJsonDto> edgarFormerNames)
        {
            if (edgarFormerNames is null)
            {
                return Array.Empty<FormerName>();
            }

            var returnDto = new FormerName[edgarFormerNames.Count];
            for (var i = 0; i < edgarFormerNames.Count; i++)
            {
                returnDto[i] = new FormerName(
                    edgarFormerNames[i].Name,
                    edgarFormerNames[i].From,
                    edgarFormerNames[i].To);
            }

            return returnDto;
        }
    }
}