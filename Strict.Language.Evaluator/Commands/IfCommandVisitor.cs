using System;
using Strict.Context;
using Strict.Language;
using Strict.Language.Commands;

namespace Strict.Evaluator.Commands
{
	public static class IfCommandVisitor
	{
		public static void VisitCommand(IfCommand ifCommand, IVisitor visitor, IContext context) =>
			throw new NotImplementedException();
	}
}