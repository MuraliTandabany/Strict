using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Strict.Context;
using Strict.Exceptions;
using Strict.Language;

namespace Strict.Evaluator.Functions
{
	public class Range : IEnumerable
	{
		public int From { get; }
		public int To { get; }
		public int Step { get; }

		public Range(int to) : this(0, to, 1) { }
		public Range(int from, int to) : this(from, to, 1) { }

		public Range(int from, int to, int step)
		{
			if (step == 0)
				throw new ValueErrorException("range() arg 3 must not be zero");
			From = from;
			To = to;
			Step = step;
		}

		public IEnumerator GetEnumerator()
		{
			if (Step < 0)
				for (var k = From; k > To; k += Step)
					yield return k;
			else
				for (var k = From; k < To; k += Step)
					yield return k;
		}

		public IList ToList() => this.Cast<object>().ToList();
	}

	public class RangeFunction : IFunction
	{
		public object Visit(IContext context, IList<object> arguments,
			IDictionary<string, object> namedArguments) =>
			arguments.Count switch
			{
				1 => new Range(Numbers.ToInteger(arguments[0])),
				2 => new Range(Numbers.ToInteger(arguments[0]), Numbers.ToInteger(arguments[1])),
				3 => new Range(Numbers.ToInteger(arguments[0]), Numbers.ToInteger(arguments[1]),
					Numbers.ToInteger(arguments[2])),
				0 => throw new TypeErrorException("range expected 1 arguments, got 0"),
				_ => throw new TypeErrorException(
					string.Format("range expected at most 3 arguments, got {0}", arguments.Count))
			};
	}
}