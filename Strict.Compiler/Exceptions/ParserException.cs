using System;

namespace Strict.Compiler.Exceptions
{
	public abstract class ParserException : Exception
	{
		protected ParserException(string msg) : base(msg) { }
	}
}