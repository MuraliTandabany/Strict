using System;

namespace Strict.Exceptions
{
    public class NameErrorException : Exception
    {
        public NameErrorException(string message) : base(message) { }
    }
}