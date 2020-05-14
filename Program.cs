using System;
using System.IO;
using ZonePivotValidation.Deserialization;
using ZonePivotValidation.DirCrawler;

namespace ZonePivotValidation
{
    class Program
    {
        static void Main(string[] args)
        {
            bool invalidFile = true;
            var path = string.Empty;
            YamlDeserializer deserializer = null;
            var message = "Enter full path to the zone-pivot .yml definition file.";

            while (invalidFile)
            {
                Console.BackgroundColor = ConsoleColor.Black;
                Console.ForegroundColor = ConsoleColor.Blue;
                Console.WriteLine(message);
                Console.ResetColor();

                path = Console.ReadLine();
                if (path.EndsWith(".yml"))
                {
                    try
                    {
                        deserializer = new YamlDeserializer(path);
                        deserializer.DeserializeYaml();

                        Console.WriteLine();
                        Console.BackgroundColor = ConsoleColor.Black;
                        Console.ForegroundColor = ConsoleColor.Blue;
                        Console.WriteLine("-- Zone-pivot definition loaded successfully; displaying keys...");
                        Console.ResetColor();
                        Console.WriteLine();

                        foreach (string key in deserializer.ZoneGroupsKeyHash.Keys)
                        {
                            Console.WriteLine(key);
                        }
                        invalidFile = false;
                    }
                    catch (FileNotFoundException ex)
                    {
                        Console.WriteLine();
                        Console.BackgroundColor = ConsoleColor.Black;
                        Console.ForegroundColor = ConsoleColor.Blue;
                        message = "Error: file not found for zone-pivot .yml definition. Ensure your path is correct and enter again.";
                        Console.ResetColor();
                    }
                }
                else
                {
                    Console.WriteLine();
                    Console.BackgroundColor = ConsoleColor.Black;
                    Console.ForegroundColor = ConsoleColor.Blue;
                    message = "Error: Path does not end in a .yml extension. Ensure your path is correct and enter again.";
                    Console.ResetColor();
                }
            }

            Console.WriteLine();
            Console.BackgroundColor = ConsoleColor.Black;
            Console.ForegroundColor = ConsoleColor.Blue;
            Console.WriteLine("Enter full path to the directory to run recursive validation against the zone-pivot definition .yml file.");
            Console.ResetColor();
            Console.WriteLine();

            var dirPath = Console.ReadLine();
            var dirCrawler = new DirectoryCrawler(dirPath);
            dirCrawler.RecursiveCrawlFiles(deserializer);
        }
    }
}
