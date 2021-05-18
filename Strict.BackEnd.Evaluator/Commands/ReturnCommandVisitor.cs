using System;
using Strict.Context;
using Strict.Language;
using Strict.Language.Commands;

namespace Strict.BackEnd.Evaluator.Commands
{
	public static class ReturnCommandVisitor
	{
		public static void
			VisitCommand(ReturnCommand returnCommand, IVisitor visitor, IContext context) =>
			throw new NotImplementedException();
	}
}