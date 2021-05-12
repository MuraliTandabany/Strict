namespace Strict.Compiler.Exceptions
{
	public class ExpectedTokenException : ParserException
	{
		public ExpectedTokenException(string token) : base($"Expected '{token}'") { }
	}
}