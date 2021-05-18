using Strict.Context;
using Strict.Language;
using Strict.Language.Commands;
using Strict.Machine;

namespace Strict.Evaluator.Commands
{
	public static class CompositeCommandVisitor
	{
		public static void VisitCommand(CompositeCommand compositeCommand, IVisitor visitor,
			IContext context)
		{
			var environment = context as StrictEnvironment;
			foreach (var command in compositeCommand.Commands)
			{
				command.Accept(visitor, context);
				if (environment != null && (environment.HasReturnValue || environment.WasContinue ||
					environment.WasBreak))
					break;
			}
		}
	}
}