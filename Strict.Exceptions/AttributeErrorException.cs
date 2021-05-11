using System;

namespace Strict.Exceptions
{
	public class AttributeErrorException : Exception
	{
		public AttributeErrorException(string message) : base(message) { }
	}
}