using Strict.Exceptions;

namespace Strict.Compiler.Exceptions
{
	public class UnexpectedTokenException : SyntaxErrorException
	{
		public UnexpectedTokenException(string text) : base($"Unexpected '{text}'") { }
	}
}