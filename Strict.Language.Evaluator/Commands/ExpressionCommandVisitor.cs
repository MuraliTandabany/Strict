using System;
using Strict.Context;
using Strict.Language;
using Strict.Language.Commands;

namespace Strict.Evaluator.Commands
{
	public static class ExpressionCommandVisitor
	{
		public static void VisitCommand(ExpressionCommand expressionCommand, IVisitor visitor,
			IContext context) =>
			throw new NotImplementedException();
	}
}