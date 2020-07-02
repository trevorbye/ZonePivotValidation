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

        public bool IsPrValid()
        {
            bool validPr = true;
            

            return validPr;
        }
    }
}
