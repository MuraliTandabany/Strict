using System.Collections.Generic;
using Strict.Context;

namespace Strict.Language
{
	public interface IObject : IValues
	{
		object Invoke(string name, IContext context, IList<object> arguments,
			IDictionary<string, object> namedArguments);
	}
}