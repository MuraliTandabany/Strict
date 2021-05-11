using System;

namespace Strict.Exceptions
{
    public class TypeErrorException : Exception
    {
        public TypeErrorException(string message) : base(message) { }
    }
}