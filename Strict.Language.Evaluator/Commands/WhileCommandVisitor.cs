using System;
using Strict.Context;
using Strict.Language;
using Strict.Language.Commands;

namespace Strict.Evaluator.Commands
{
	public static class WhileCommandVisitor
	{
		public static void
			VisitCommand(WhileCommand whileCommand, IVisitor visitor, IContext context) =>
			throw new NotImplementedException();
	}
}