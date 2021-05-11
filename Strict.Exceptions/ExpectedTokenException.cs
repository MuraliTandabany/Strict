namespace Strict.Compiler
{
	public class ExpectedTokenException : ParserException
	{
		public ExpectedTokenException(string token) : base($"Expected '{token}'") { }
	}
}