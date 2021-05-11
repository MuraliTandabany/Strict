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
                    var parser = new Parser(File.OpenText(file.FullName));
                    var commands = parser.CompileCommandList();
                    var commandsJsonResult = JsonConvert.SerializeObject(commands);
                }
            }
        }

        private static void Main(string[] args) => TestFiles.ParseStrictSourcesTest();
    }
}