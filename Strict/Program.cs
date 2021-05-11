using System;
using System.Diagnostics;
using System.IO;
using Newtonsoft.Json;
using Strict.Compiler;

namespace Strict
{
    internal class Program
    {
        private static class TestFiles
        {
            public static void ParseStrictSourcesTest()
            {
                var directoryInfo = new DirectoryInfo("StrictFileTests");
                foreach (var file in directoryInfo.GetFiles("*.strict"))
                {

                    var sw = Stopwatch.StartNew();
                    var parser = new Parser(File.OpenText(file.FullName));
                    var commands = parser.CompileCommandList();
                    var elapsedTime = sw.Elapsed;

                    var commandsJsonResult = JsonConvert.SerializeObject(commands, Formatting.Indented);

                    Console.WriteLine("File: {0}", file.Name);
                    Console.WriteLine("Total used time to build: {0}", elapsedTime);
                    Console.WriteLine("Source:");
                    Console.WriteLine();
                    Console.WriteLine(File.ReadAllText(file.FullName));
                    Console.WriteLine();
                    Console.WriteLine("Json from parsing:");
                    Console.WriteLine();
                    Console.WriteLine(commandsJsonResult);
                    Console.WriteLine();
                    Console.WriteLine("".PadLeft(Console.WindowWidth, '-'));
                    Console.WriteLine("".PadLeft(Console.WindowWidth, '-'));
                    Console.WriteLine("".PadLeft(Console.WindowWidth, '-'));
                    Console.WriteLine();
                }
            }
        }

        private static void Main(string[] args) => TestFiles.ParseStrictSourcesTest();
    }
}