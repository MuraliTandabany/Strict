using Strict.Exceptions;

namespace Strict.Compiler
{
	public class UnexpectedEndOfInputException : SyntaxErrorException
	{
		public UnexpectedEndOfInputException() : base("Unexpected End of Input") { }
	}
}