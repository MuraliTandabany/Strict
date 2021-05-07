using System;

namespace Strict.Compiler
{
    public abstract class ParserException : Exception
    {
        protected ParserException(string msg) : base(msg) { }
    }
}