using System.Collections.Generic;

namespace Strict.Language
{
    public interface IValues
    {
        object GetValue(string name);

        void SetValue(string name, object value);

        bool HasValue(string name);

        ICollection<string> GetNames();
    }
}