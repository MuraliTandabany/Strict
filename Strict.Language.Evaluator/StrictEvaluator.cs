using System;
using System.IO;
using Strict.Evaluator.Functions;
using Strict.Language;
using Strict.Machine;

namespace Strict.Evaluator
{
	public class StrictEvaluator : IStrictMachine
	{
		public IVisitor Visitor { get; } = new EvaluatorVisitor();
		public StrictEnvironment Environment { get; set; } = new();
		public TextReader Input { get; set; } = Console.In;
		public TextWriter Output { get; set; } = Console.Out;

		public StrictEvaluator()
		{
			Environment.SetValue("print", new PrintFunction(this));
			Environment.SetValue("range", new RangeFunction());
		}
	}
}