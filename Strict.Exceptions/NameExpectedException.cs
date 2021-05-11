namespace Strict.Compiler
{
	public class NameExpectedException : ParserException
	{
		public NameExpectedException() : base("A name was expected") { }
	}
}