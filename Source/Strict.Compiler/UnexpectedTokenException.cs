using System.Globalization;
using Strict.Exceptions;

namespace Strict.Compiler
{
    public class UnexpectedTokenException : SyntaxError
    {
        public UnexpectedTokenException(Token token) : base($"Unexpected '{token.Value}'") { }
        public UnexpectedTokenException(string text) : base($"Unexpected '{text}'") { }
    }
}