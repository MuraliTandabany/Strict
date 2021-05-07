using System;

namespace Strict.Exceptions
{
    public class TypeError : Exception
    {
        public TypeError(string message) : base(message) { }
    }
}