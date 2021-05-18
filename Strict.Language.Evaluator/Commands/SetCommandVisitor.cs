using System;
using Strict.Context;
using Strict.Language;
using Strict.Language.Commands;

namespace Strict.Evaluator.Commands
{
	public static class SetCommandVisitor
	{
		public static void VisitCommand(SetCommand setCommand, IVisitor visitor, IContext context) =>
			throw new NotImplementedException();
	}
}