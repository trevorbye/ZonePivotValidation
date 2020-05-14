using System;
using System.Collections.Generic;
using System.Text;
using ZonePivotValidation.Deserialization;
using ZonePivotValidation.DirCrawler;

namespace ZonePivotValidation.Validation
{
    class Validator
    {
        public MarkdownFileHandler MarkdownFileHandler { get; set; }
        public YamlDeserializer Deserializer { get; set; }

        public bool DefinitionForUsedPivotGroup { get; set; }
        public HashSet<string> PivotIdsNotInPivotGroupDefinition { get; set; }
        public HashSet<string> PivotIdsNotInMarkdownFile { get; set; }
        public bool PassesValidation { get; set; }

        public Validator(MarkdownFileHandler fileHandler, YamlDeserializer deserializer)
        {
            MarkdownFileHandler = fileHandler;
            Deserializer = deserializer;

            PivotIdsNotInPivotGroupDefinition = new HashSet<string>();
            PivotIdsNotInMarkdownFile = new HashSet<string>();
            PassesValidation = true;
            DefinitionForUsedPivotGroup = true;
        }

        public void RunBasicMembershipValidation()
        {
            if (!Deserializer.ZoneGroupsKeyHash.ContainsKey(MarkdownFileHandler.ZonePivotGroupId)) 
            {
                // this means that the markdown file references a pivot group id that isn't defined in the .yml group definitions at all
                PassesValidation = false;
                DefinitionForUsedPivotGroup = false;
            }
            else
            {
                foreach (string markdownPivotLang in MarkdownFileHandler.PivotLangs)
                {
                    if (!Deserializer.ZoneGroupsKeyHash[MarkdownFileHandler.ZonePivotGroupId].PivotIds.Contains(markdownPivotLang))
                    {
                        // this means that the markdown file attempts to use a zone pivot id that isn't defined in the zone-pivot group
                        PivotIdsNotInPivotGroupDefinition.Add(markdownPivotLang);
                        PassesValidation = false;
                    }
                }

                foreach (string zonePivotLangId in Deserializer.ZoneGroupsKeyHash[MarkdownFileHandler.ZonePivotGroupId].PivotIds)
                {
                    if (!MarkdownFileHandler.PivotLangs.Contains(zonePivotLangId))
                    {
                        // this means that there is a defined zone pivot id, that is not referenced in the markdown file
                        // e.g. the zone would be empty when rendered in docs
                        PivotIdsNotInMarkdownFile.Add(zonePivotLangId);
                        PassesValidation = false;
                    }
                }
            }
        }
        
    }
}
