using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using ZonePivotValidation.Deserialization;
using ZonePivotValidation.Validation;

namespace ZonePivotValidation.DirCrawler
{
    class DirectoryCrawler
    {
        private string _startDir;

        public DirectoryCrawler(string startDir)
        {
            this._startDir = startDir;
        }

        public void RecursiveCrawlFiles(YamlDeserializer deserializer)
        {
            foreach (string path in Directory.EnumerateFiles(this._startDir, "*.md", SearchOption.AllDirectories))
            {
                var fileHandler = new MarkdownFileHandler(path);
                fileHandler.ReadMarkdownLines();
                if (fileHandler.HasZonePivots)
                {
                    // do validation
                    var validator = new Validator(fileHandler, deserializer);
                    validator.RunBasicMembershipValidation();

                    if (validator.PassesValidation)
                    {
                        Console.Write($"{fileHandler.FilePath}: ");
                        Console.BackgroundColor = ConsoleColor.Black;
                        Console.ForegroundColor = ConsoleColor.Green;
                        Console.Write("PASS");
                        Console.ResetColor();
                        Console.WriteLine();
                    }
                    else
                    {
                        Console.Write($"{fileHandler.FilePath}: ");
                        Console.BackgroundColor = ConsoleColor.Black;
                        Console.ForegroundColor = ConsoleColor.Red;
                        Console.Write("FAIL");
                        Console.ResetColor();
                        Console.WriteLine();
                        Console.WriteLine();

                        if (!validator.DefinitionForUsedPivotGroup)
                        {
                            Console.Write("    The pivot group ");

                            Console.BackgroundColor = ConsoleColor.Black;
                            Console.ForegroundColor = ConsoleColor.DarkYellow;
                            Console.Write($"{validator.MarkdownFileHandler.ZonePivotGroupId}");
                            Console.ResetColor();

                            Console.Write(" is not defined in the zone definitions .yml file.");
                            Console.WriteLine();
                            Console.WriteLine();
                        }
                        else 
                        {
                            if (validator.PivotIdsNotInPivotGroupDefinition.Count > 0)
                            {
                                foreach (string id in validator.PivotIdsNotInPivotGroupDefinition)
                                {
                                    Console.Write("    The zone-pivot id ");
                                    Console.BackgroundColor = ConsoleColor.Black;
                                    Console.ForegroundColor = ConsoleColor.Cyan;
                                    Console.Write($"{id}");
                                    Console.ResetColor();

                                    Console.Write(" is not defined in the pivot-group ");

                                    Console.BackgroundColor = ConsoleColor.Black;
                                    Console.ForegroundColor = ConsoleColor.DarkYellow;
                                    Console.Write($"{validator.MarkdownFileHandler.ZonePivotGroupId}.");
                                    Console.ResetColor();

                                    Console.WriteLine();
                                }
                                Console.WriteLine();
                            }

                            if (validator.PivotIdsNotInMarkdownFile.Count > 0)
                            {
                                foreach (string id in validator.PivotIdsNotInMarkdownFile)
                                {
                                    Console.Write("    The zone-pivot id ");
                                    Console.BackgroundColor = ConsoleColor.Black;
                                    Console.ForegroundColor = ConsoleColor.Cyan;
                                    Console.Write($"{id}");
                                    Console.ResetColor();

                                    Console.Write(" is defined in the pivot-group ");
                                    Console.BackgroundColor = ConsoleColor.Black;
                                    Console.ForegroundColor = ConsoleColor.DarkYellow;
                                    Console.Write($"{validator.MarkdownFileHandler.ZonePivotGroupId}");
                                    Console.ResetColor();

                                    Console.Write(", but not used in ");
                                    string pathStr = validator.MarkdownFileHandler.FilePath;
                                    pathStr = pathStr.Substring(pathStr.LastIndexOf("\\") + 1);
                                    Console.Write($"{pathStr}.");
                                    
                                    Console.WriteLine();
                                }
                                Console.WriteLine();
                            }
                        }
                    }
                }
            }
        }
    }
}
