using Strict.Context;
using Strict.Language;
using Strict.Language.Commands;

namespace Strict.BackEnd.Evaluator.Commands
{
	public static class IfCommandVisitor
	{
		public static void VisitCommand(IfCommand ifCommand, IVisitor visitor, IContext context)
		{
			if (Predicates.IsFalse(ifCommand.Condition.Accept(visitor, context)))
			{
				if (ifCommand.ElseCommand == null)
					return;
				ifCommand.ElseCommand.Accept(visitor, context);
			}
			else
			{
				ifCommand.ThenCommand.Accept(visitor, context);
			}
		}
	}
}