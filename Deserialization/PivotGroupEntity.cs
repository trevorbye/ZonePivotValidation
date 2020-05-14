using System;
using System.Collections.Generic;
using System.Text;

namespace ZonePivotValidation.Deserialization
{
    class PivotEntity
    {
        public string id { get; set; }
        public string title { get; set; }
    }

    class PivotGroupEntity
    {
        public string id { get; set; }
        public string title { get; set; }
        public string prompt { get; set; }
        public List<PivotEntity> pivots { get; set; }
    }

    class HashedPivotGroupEntity
    {
        public string Id { get; set; }
        public string Title { get; set; }
        public string Prompt { get; set; }
        public HashSet<string> PivotIds { get; set; }

        public HashedPivotGroupEntity(string id, string title, string prompt)
        {
            Id = id;
            Title = title;
            Prompt = prompt;
            PivotIds = new HashSet<string>();
        }
    }

    class ZonePivotFile
    {
        public List<PivotGroupEntity> groups { get; set; }
    }
}
