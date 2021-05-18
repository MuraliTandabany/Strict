using System.Collections.Generic;
using System.Linq;
using Strict.Context;
using Strict.Language;

namespace Strict.Evaluator
{
	public class DefinedClass : IType, IFunction, IContext
	{
		public string Name { get; }

		public IContext GlobalContext { get; }

		public IList<IType> Bases { get; }

		public void SetMethod(string name, IFunction method) => values[name] = method;
		private const string ConstructorName = "__constructor_";
		private readonly IDictionary<string, object> values = new Dictionary<string, object>();

		public DefinedClass(string name) : this(name, null, null) { }

		public DefinedClass(string name, IContext global) : this(name, null, global) { }

		public DefinedClass(string name, IList<IType> bases) : this(name, bases, null) { }

		public DefinedClass(string name, IList<IType> bases, IContext global)
		{
			Name = name;
			GlobalContext = global;
			Bases = bases;
		}

		public IFunction GetMethod(string name)
		{
			if (values.ContainsKey(name))
			{
				var method = values[name] as IFunction;
				return method;
			}
			return Bases?.Select(type => type.GetMethod(name)).FirstOrDefault(method => method != null);
		}

		public bool HasMethod(string name)
		{
			var hasMethod = values.ContainsKey(name) && values[name] is IFunction;
			return hasMethod || Bases != null && Bases.Any(type => type.HasMethod(name));
		}

		public object Visit(IContext context, IList<object> arguments,
			IDictionary<string, object> namedArguments)
		{
			var dynamicObject = new DynamicObject(this);
			if (!HasMethod(ConstructorName))
				return dynamicObject;
			var constructor = GetMethod(ConstructorName);
			IList<object> args = new List<object> { dynamicObject };
			if (arguments is { Count: > 0 })
				foreach (var arg in arguments)
					args.Add(arg);
			constructor.Visit(context, args, namedArguments);
			return dynamicObject;
		}

		public object GetValue(string name) =>
			values.ContainsKey(name)
				? values[name]
				: Bases != null
					? (from type in Bases where type.HasValue(name) select type.GetValue(name)).
					FirstOrDefault()
					: null;

		public void SetValue(string name, object value) => values[name] = value;

		public bool HasValue(string name) =>
			values.ContainsKey(name) || Bases != null && Bases.Any(type => type.HasValue(name));

		public ICollection<string> GetNames() => values.Keys.ToList();

		public override string ToString() => $"<class '{Name}'>";
	}
}