using System.Collections.Generic;
using System.Linq;
using Strict.Context;
using Strict.Exceptions;
using Strict.Language;
using Strict.Language.Commands;
using Strict.Machine;

namespace Strict.BackEnd.Evaluator
{
	public class DefinedFunction : DynamicObject, IFunction, IValues
	{
		public IList<Parameter> Parameters { get; }
		public int NumberMinimumParameters { get; }
		public int NumberOfMaxParameters { get; }
		public int NumberOfParameters { get; }
		public bool HasDefault { get; }
		public bool HasList { get; }
		public string Name { get; set; }
		public ICommand Body { get; set; }

		public DefinedFunction(string name, IList<Parameter> parameters, ICommand body,
			IContext context) : base(null)
		{
			Name = name;
			Parameters = parameters;
			Body = body;
			if (parameters == null)
				return;
			NumberOfParameters = parameters.Count;
			NumberOfMaxParameters = parameters.Count;
			foreach (var parameter in parameters)
			{
				if (parameter.DefaultValue != null)
					HasDefault = true;
				if (parameter.IsList)
				{
					HasList = true;
					NumberOfMaxParameters = int.MaxValue;
				}
				if (!HasDefault && !HasList)
					NumberMinimumParameters++;
			}
		}

		public object Visit(IContext context, IList<object> arguments,
			IDictionary<string, object> namedArguments)
		{
			var newContext = new StrictEnvironment(context);
			var argsCount = 0;
			if (arguments != null)
				argsCount = arguments.Count;
			if (argsCount < NumberMinimumParameters || argsCount > NumberOfMaxParameters)
				throw new TypeErrorException(
					$"{Name}() takes {(HasDefault ? "at least" : "exactly")} {NumberMinimumParameters} positional argument{(NumberMinimumParameters == 1 ? string.Empty : "s")} ({argsCount} given)");
			if (namedArguments != null)
				foreach (var namArg in namedArguments)
					newContext.SetValue(namArg.Key, namArg.Value);
			if (Parameters != null)
			{
				int k;
				for (k = 0; k < Parameters.Count; k++)
					if (arguments != null && arguments.Count > k)
					{
						if (namedArguments != null && namedArguments.ContainsKey(Parameters[k].Name))
							throw new TypeErrorException(
								$"{Name}() got multiple values for keyword argument '{Parameters[k].Name}'");
						newContext.SetValue(Parameters[k].Name, Parameters[k].IsList
							? GetSublist(arguments, k)
							: arguments[k]);
					}
					else if (Parameters[k].IsList)
					{
						newContext.SetValue(Parameters[k].Name, Parameters[k].DefaultValue == null
							? new List<object>()
							: Parameters[k].DefaultValue);
						break;
					}
					else if (namedArguments == null || !namedArguments.ContainsKey(Parameters[k].Name))
					{
						newContext.SetValue(Parameters[k].Name, Parameters[k].DefaultValue);
					}
			}
			Body.Accept(newContext.Visitor, newContext);
			return newContext.HasReturnValue
				? newContext.ReturnValue
				: null;
		}

		public override string ToString() => $"<function {Name}>";

		private static IList<object> GetSublist(IList<object> list, int from) =>
			list.Skip(from).ToList();
	}
}