using Strict.Exceptions;

namespace Strict.Compiler
{
    public class UnexpectedEndOfInputException : SyntaxError
    {
        public UnexpectedEndOfInputException() : base("Unexpected End of Input") { }
    }
}