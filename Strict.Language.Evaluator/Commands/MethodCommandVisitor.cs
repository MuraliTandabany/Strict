using System;
using Strict.Context;
using Strict.Language;
using Strict.Language.Commands;

namespace Strict.Evaluator.Commands
{
	public static class MethodCommandVisitor
	{
		public static void
			VisitCommand(MethodCommand methodCommand, IVisitor visitor, IContext context) =>
			throw new NotImplementedException();
	}
}