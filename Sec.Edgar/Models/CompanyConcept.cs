using System.Collections.Generic;
using Sec.Edgar.Enums;
using Sec.Edgar.Models.Edgar;

namespace Sec.Edgar.Models
{
    public class CompanyConcept
    {
        internal CompanyConcept(CompanyConceptJsonDto jsonDto)
        {
            CentralIndexKey = jsonDto.CentralIndexKey;
            Taxonomy = jsonDto.Taxonomy;
            Tag = jsonDto.Tag;
            Label = jsonDto.Label;
            Description = jsonDto.Description;
            EntityName = jsonDto.EntityName;
        
            foreach (var unit in jsonDto.Units)
            {
                var values = new CompanyConceptRecord[unit.Value.Length];
                for (var i = 0; i < values.Length; i++)
                {
                    values[i] = new CompanyConceptRecord(unit.Value[i]);
                }

                if (!Units.ContainsKey(unit.Key))
                {
                    Units.Add(unit.Key, values);
                }
            }
        }
    
        public int CentralIndexKey { get; }

        public Taxonomy Taxonomy { get; }

        public string Tag { get; }

        public string Label { get; }

        public string Description { get; }

        public string EntityName { get; }

        public Dictionary<string, CompanyConceptRecord[]> Units { get; } = new Dictionary<string, CompanyConceptRecord[]>();
    }
}