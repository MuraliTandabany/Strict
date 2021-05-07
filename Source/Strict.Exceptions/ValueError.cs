using System;

namespace Strict.Exceptions
{
    public class ValueError : Exception
    {
        public ValueError(string message) : base(message) { }
    }
}