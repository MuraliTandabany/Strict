using System.Collections.Generic;
using Strict.Context;
using Strict.BackEnd.Evaluator.Utilities;
using Strict.Language;

namespace Strict.BackEnd.Evaluator.Functions
{
	public class PrintFunction : IFunction
	{
		private StrictEvaluator Machine { get; }
		public PrintFunction(StrictEvaluator machine) => Machine = machine;

		public object Visit(IContext context, IList<object> arguments,
			IDictionary<string, object> namedArguments)
		{
			var separator = namedArguments != null && namedArguments.ContainsKey("sep")
				? (string)namedArguments["sep"]
				: " ";
			var end = namedArguments != null && namedArguments.ContainsKey("end")
				? (string)namedArguments["end"]
				: null;
			if (arguments != null)
			{
				var narg = 0;
				foreach (var argument in arguments)
				{
					if (narg != 0)
						Machine.Output.Write(separator);
					Machine.Output.Write(ValueUtilities.AsPrintString(argument));
					narg++;
				}
			}
			if (end != null)
				Machine.Output.Write(end);
			else
				Machine.Output.WriteLine();
			return null;
		}
	}
}