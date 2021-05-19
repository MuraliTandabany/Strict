using Strict.Context;
using Strict.Language;
using Strict.Language.Commands;

namespace Strict.BackEnd.Evaluator.Commands
{
	public static class LetCommandVisitor
	{
		public static void VisitCommand(LetCommand letCommand, IVisitor visitor, IContext context) =>
			context.SetValue(letCommand.Target, letCommand.Expression.Accept(visitor, context));
	}
}