using System;

namespace Strict.Exceptions
{
    public class SyntaxError : Exception
    {
        public SyntaxError(string message) : base(message) { }
    }
}