using System;
using Strict.Context;
using Strict.Language;
using Strict.Language.Commands;

namespace Strict.Evaluator.Commands
{
	public static class ForCommandVisitor
	{
		public static void VisitCommand(ForCommand forCommand, IVisitor visitor, IContext context) =>
			throw new NotImplementedException();
	}
}