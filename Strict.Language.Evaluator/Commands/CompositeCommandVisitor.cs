using System;
using Strict.Context;
using Strict.Language;
using Strict.Language.Commands;

namespace Strict.Evaluator.Commands
{
	public static class CompositeCommandVisitor
	{
		public static void VisitCommand(CompositeCommand compositeCommand, IVisitor visitor,
			IContext context) =>
			throw new NotImplementedException();
	}
}