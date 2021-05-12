using Strict.Exceptions;

namespace Strict.Compiler.Exceptions
{
	public class UnexpectedEndOfInputException : SyntaxErrorException
	{
		public UnexpectedEndOfInputException() : base("Unexpected End of Input") { }
	}
}