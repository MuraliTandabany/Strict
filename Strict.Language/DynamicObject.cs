using System.Collections.Generic;
using System.Linq;
using Strict.Context;
using Strict.Exceptions;

namespace Strict.Language
{
	public class DynamicObject : IObject
	{
		private IDictionary<string, object> Values { get; } = new Dictionary<string, object>();

		public DynamicObject(IType _class) => Class = _class;

		public IType Class { get; }

		public object GetValue(string name) =>
			Values.ContainsKey(name)
				? Values[name]
				: Class.GetValue(name);

		public void SetValue(string name, object value) => Values[name] = value;

		public bool HasValue(string name) =>
			Values.ContainsKey(name) || Class != null && Class.HasValue(name);

		public object Invoke(string name, IContext context, IList<object> arguments,
			IDictionary<string, object> namedArguments)
		{
			var value = GetValue(name);
			var method = value as IFunction;
			if (method == null)
				throw new TypeErrorException($"'{Types.GetTypeName(value)}' object is not callable");
			IList<object> args = new List<object> { this };
			if (arguments == null || arguments.Count <= 0)
				return method.Visit(context, args, namedArguments);
			foreach (var arg in arguments)
				args.Add(arg);
			return method.Visit(context, args, namedArguments);
		}

		public ICollection<string> GetNames() => Values.Keys.ToList();
	}
}