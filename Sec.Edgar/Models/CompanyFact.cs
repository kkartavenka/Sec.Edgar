using System.Collections.Generic;
using Sec.Edgar.Enums;
using Sec.Edgar.Models.Edgar;

namespace Sec.Edgar.Models
{
    public class CompanyFact
    {
        internal CompanyFact(CompanyFactJsonDto jsonDto)
        {
            CentralIndexKey = jsonDto.CentralIndexKey;
            EntityName = jsonDto.EntityName;
            foreach (var jsonDtoFact in jsonDto.Facts)
            {
                var taxonomyDict = new Dictionary<string, FactModel>();
                foreach (var factModelJsonDto in jsonDtoFact.Value)
                {
                    if (!taxonomyDict.ContainsKey(factModelJsonDto.Key))
                    {
                        taxonomyDict.Add(factModelJsonDto.Key, new FactModel(factModelJsonDto.Value));
                    }
                }

                if (!Facts.ContainsKey(jsonDtoFact.Key))
                {
                    Facts.Add(jsonDtoFact.Key, taxonomyDict);
                }
            }
        }
    
        public int CentralIndexKey { get; set; }
        public string EntityName { get; set; }

        public Dictionary<Taxonomy, Dictionary<string, FactModel>> Facts { get; set; } =
            new Dictionary<Taxonomy, Dictionary<string, FactModel>>();
    }
}