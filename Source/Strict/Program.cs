using System;
using Strict.Compiler;

namespace Strict
{
    class Program
    {
        static void Main(string[] args)
        {
            var parser = new Parser(@"Samples\HelloWorld.strict");
            // TODO: var commands = parser.CompileCommandList();
        }
    }
}
