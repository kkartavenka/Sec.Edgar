using System.Collections.Generic;
using System.Collections.ObjectModel;
using Sec.Edgar.Models.Edgar;

namespace Sec.Edgar.Models
{
    public class FactModel
    {
        internal FactModel(FactModelJsonDto factModelJsonDto)
        {
            Label = factModelJsonDto.Label;
            Description = factModelJsonDto.Description;

            var mappedDictionary = new Dictionary<string, CompanyConceptRecord[]>();
            foreach (var unitValue in factModelJsonDto.Units)
            {
                var valueArray = new CompanyConceptRecord[unitValue.Value.Length];
                for (var i = 0; i < valueArray.Length; i++)
                {
                    valueArray[i] = new CompanyConceptRecord(unitValue.Value[i]);
                }

                if (!mappedDictionary.ContainsKey(unitValue.Key))
                {
                    mappedDictionary.Add(unitValue.Key, valueArray);
                }
            }

            Units = new ReadOnlyDictionary<string, CompanyConceptRecord[]>(mappedDictionary);
        }
    
        public string Label { get; }
        public string Description { get; }
        public ReadOnlyDictionary<string, CompanyConceptRecord[]> Units { get; }
    }
}