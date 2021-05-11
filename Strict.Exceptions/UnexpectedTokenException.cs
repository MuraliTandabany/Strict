using Strict.Exceptions;

namespace Strict.Compiler
{
	public class UnexpectedTokenException : SyntaxErrorException
	{
		public UnexpectedTokenException(string text) : base($"Unexpected '{text}'") { }
	}
}