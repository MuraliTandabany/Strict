using System;
using Strict.Context;
using Strict.Language;
using Strict.Language.Commands;

namespace Strict.Evaluator.Commands
{
	public static class ClassCommandVisitor
	{
		public static void
			VisitCommand(ClassCommand classCommand, IVisitor visitor, IContext context) =>
			throw new NotImplementedException();
	}
}