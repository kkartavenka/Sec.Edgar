using System;
using Sec.Edgar.Enums;
using Sec.Edgar.Models.Edgar;

namespace Sec.Edgar.Models
{
    public class CompanyConceptRecord
    {
        internal CompanyConceptRecord(CompanyConceptRecordJsonDto jsonDto)
        {
            StartDate = jsonDto.StartDate;
            EndDate = jsonDto.EndDate;
            Value = jsonDto.Value;
            AccessionNumber = jsonDto.AccessionNumber;
            FiscalYear = jsonDto.FiscalYear;
            FiscalPeriod = jsonDto.FiscalPeriod;
            Form = jsonDto.Form;
            Filed = jsonDto.Filed;
            Frame = jsonDto.Frame;
        }
    
        public DateTime? StartDate { get; }
        public DateTime? EndDate { get; }
        public double? Value { get; }
        public string AccessionNumber { get; }
        public int FiscalYear { get; }
        public FiscalPeriod FiscalPeriod { get; }
        public FormType Form { get; }
        public DateTime? Filed { get; }
        public string Frame { get; }
    }
}