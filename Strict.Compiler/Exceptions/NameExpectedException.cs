namespace Strict.Compiler.Exceptions
{
	public class NameExpectedException : ParserException
	{
		public NameExpectedException() : base("A name was expected") { }
	}
}