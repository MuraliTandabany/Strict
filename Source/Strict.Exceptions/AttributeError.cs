using System;

namespace Strict.Exceptions
{
    public class AttributeError : Exception
    {
        public AttributeError(string message) : base(message) { }
    }
}