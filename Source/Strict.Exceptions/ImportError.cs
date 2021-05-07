using System;

namespace Strict.Exceptions
{
    public class ImportError : Exception
    {
        public ImportError(string message) : base(message) { }
    }
}