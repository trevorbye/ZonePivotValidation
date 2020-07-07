using System;
using System.Collections.Generic;
using System.Text;
using ZonePivotValidation.Deserialization;

namespace ZonePivotValidation.Validation
{
    class PrValidator
    {
        public SearchableZonePivotFile MasterFile { get; set; }
        public SearchableZonePivotFile TreeFile { get; set; }

        public PrValidator(string pathToMasterFile, string pathToTreeFile)
        {
            MasterFile = YamlDeserializer.DeserializeYamlToSearchable(pathToMasterFile);
            TreeFile = YamlDeserializer.DeserializeYamlToSearchable(pathToTreeFile);
        }

        public List<string> GetZonePivotPrValidationErrors()
        {
            var errors = new List<string>();
            // verify all zone id's from master version exist in tree version
            foreach (string masterZoneId in MasterFile.ZoneIdsAndGroupMap.Keys)
            {
                if (TreeFile.ZoneIdsAndGroupMap.ContainsKey(masterZoneId))
                {
                    // verify that zone definition hasn't changed. If dicts are equal, pivot def is the same
                    if (MasterFile.ZoneIdsAndGroupMap[masterZoneId].Pivots != TreeFile.ZoneIdsAndGroupMap[masterZoneId].Pivots)
                    {
                        errors.Add($"The zone definition for '{masterZoneId}' has been modified.");
                    }
                }
                else 
                {
                    errors.Add($"The zone definition '{masterZoneId}' has been removed."); 
                }
            }

            if (errors.Count == 0)
            {
                return null;
            }
            return errors;
        }
    }
}
