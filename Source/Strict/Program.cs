using System;
using System.Diagnostics;
using System.Text;
using System.Threading.Tasks;
using Strict.Compiler;

namespace Strict
{
    class Program
    {
        static void Time<T>(int taskNumber, Func<T> work)
        {
            var sw = Stopwatch.StartNew();
            var result = work();
            var elapsedTime = sw.Elapsed;
            Console.WriteLine(taskNumber.ToString() + "> " + elapsedTime + ": ( Token Count , Number of Lines ) " + result);
        }

        static Tuple<int, int> TokenizeAllSrc(string src)
        {
            int tokenCnt = 0;
            int lineCount = 0;

            using (var lexer = new Lexer(src))
            {
                Token token;
                while ((token = lexer.NextToken()) != null)
                {
                    if (token.TokenType == TokenType.EndOfLine)
                        lineCount++;

                    tokenCnt++;
                }
            }

            return new Tuple<int, int>(tokenCnt, lineCount);
        }

        static void Main(string[] args)
        {
            //var parser = new Parser(@"Samples\HelloWorld.strict");
            // TODO: var commands = parser.CompileCommandList();

            var src1 = @"
for i = 0 to 10 
    for j = 0 to 20 * i 
        for k = 0 to 30 * j 

for i = 0 to 10 
    for j = 0 to 20 * i 
        for k = 0 to 30 * j 

print i j k () |> ""Hello"" <> == != . [ { | > < = / * + . , ""Hi!!!""
print i j k () |> ""Hello"" <> == != . [ { | > < = / * + . , ""Hi!!!""
print i j k () |> ""Hello"" <> == != . [ { | > < = / * + . , ""Hi!!!""


for i = 0 to 10 
    for j = 0 to 20 * i 
        for k = 0 to 30 * j 

for i = 0 to 10 
    for j = 0 to 20 * i 
        for k = 0 to 30 * j 


print i j k () |> ""Hello"" <> == != . [ { | > < = / * + . , ""Hi!!!""
print i j k () |> ""Hello"" <> == != . [ { | > < = / * + . , ""Hi!!!""
print i j k () |> ""Hello"" <> == != . [ { | > < = / * + . , ""Hi!!!""";

            var sb = new StringBuilder();
            for (int i = 0; i < short.MaxValue; i++)
                sb.Append(src1);
            var textCode = sb.ToString();

            Parallel.For(0, 1, (i) => Time(i, () => TokenizeAllSrc(textCode)));
        }
    }
}
