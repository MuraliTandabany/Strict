using System;

namespace Strict.Exceptions
{
    public class ValueErrorException : Exception
    {
        public ValueErrorException(string message) : base(message) { }
    }
}