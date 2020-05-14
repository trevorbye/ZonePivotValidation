using System;
using System.IO;
using System.Collections.Generic;
using System.Text;
using ZonePivotValidation.Deserialization;
using YamlDotNet.Serialization;

namespace ZonePivotValidation
{
    class YamlDeserializer
    {
        private string _path;
        public Dictionary<string, HashedPivotGroupEntity> ZoneGroupsKeyHash { get; set; }

        public YamlDeserializer(string path) 
        {
            this._path = path;
        }

        public void DeserializeYaml()
        {
            var d = new Deserializer();
            var zoneFile = d.Deserialize<ZonePivotFile>(new StreamReader(this._path));
            
            // build to hashed structure for faster read spead when finding pivot groups later on during validation
            var keyHash = new Dictionary<string, HashedPivotGroupEntity>();
            foreach (PivotGroupEntity pivotGroup in zoneFile.groups)
            {
                var hashedGroup = new HashedPivotGroupEntity(pivotGroup.id, pivotGroup.title, pivotGroup.prompt);
                foreach (var entity in pivotGroup.pivots)
                {
                    hashedGroup.PivotIds.Add(entity.id);
                }
                string key = hashedGroup.Id;
                if (!keyHash.ContainsKey(key))
                {
                    keyHash.Add(key, hashedGroup);
                }
            }
            this.ZoneGroupsKeyHash = keyHash;
        }
    }
}
