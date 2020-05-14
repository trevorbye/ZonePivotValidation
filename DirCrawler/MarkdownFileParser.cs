using System;
using System.IO;
using System.Collections.Generic;
using System.Text;

namespace ZonePivotValidation.DirCrawler
{
    class MarkdownFileHandler
    {
        public string ZonePivotGroupId { get; set; }
        public HashSet<string> PivotLangs { get; set; }

        public bool HasZonePivots { get; set; }

        public string FilePath { get; set; }

        public MarkdownFileHandler(string path)
        {
            FilePath = path;
            PivotLangs = new HashSet<string>();
        }

        public void ReadMarkdownLines()
        {
            int dashCount = 0;

            using (FileStream fs = File.OpenRead(FilePath))
            using (BufferedStream bs = new BufferedStream(fs))
            using (StreamReader sr = new StreamReader(bs))
            {
                while (sr.Peek() >= 0)
                {
                    var line = sr.ReadLine().Trim();
                    if (line == "---")
                    {
                        if (dashCount == 0)
                        {
                            dashCount += 1;
                        }
                        else
                        {
                            if (ZonePivotGroupId is null)
                            {
                                HasZonePivots = false;
                                break;
                            }
                        }
                    }
                    else if (line.StartsWith("zone_pivot_groups:"))
                    {
                        var id = line.Substring(line.IndexOf(":") + 1).Trim();
                        ZonePivotGroupId = id;
                        HasZonePivots = true;
                    }
                    else if (line.StartsWith("::: zone pivot"))
                    {
                        var pivotLang = line.Substring(line.IndexOf("\"") + 1);
                        pivotLang = pivotLang.Remove(pivotLang.Length - 1);
                        PivotLangs.Add(pivotLang);
                    }
                }
            }
        }
    }
}
