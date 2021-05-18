using System.Collections.Generic;
using Strict.Context;

namespace Strict.Language
{
	public interface IFunction
	{
		object Visit(IContext context, IList<object> arguments,
			IDictionary<string, object> namedArguments);
	}
}