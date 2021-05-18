using Strict.Context;
using Strict.Language;
using Strict.Language.Commands;
using Strict.Machine;

namespace Strict.Evaluator.Commands
{
	public static class WhileCommandVisitor
	{
		public static void VisitCommand(WhileCommand whileCommand, IVisitor visitor, IContext context)
		{
			var environment = context as StrictEnvironment;
			while (!Predicates.IsFalse(whileCommand.Condition.Accept(visitor, context)))
			{
				whileCommand.Command.Accept(visitor, context);
				if (environment == null)
					continue;
				if (environment.HasReturnValue)
					return;
				if (environment.WasBreak)
				{
					environment.WasBreak = false;
					break;
				}
				if (environment.WasContinue)
					environment.WasContinue = false;
			}
		}
	}
}