using System;

namespace Strict.Exceptions
{
    public class NameError : Exception
    {
        public NameError(string message) : base(message) { }
    }
}