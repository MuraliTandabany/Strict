using System;
using Strict.Context;
using Strict.Language;
using Strict.Language.Commands;

namespace Strict.Evaluator.Commands
{
	public static class HasCommandVisitor
	{
		public static void VisitCommand(HasCommand hasCommand, IVisitor visitor, IContext context) =>
			throw new NotImplementedException();
	}
}