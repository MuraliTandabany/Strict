using System;
using Strict.Context;
using Strict.Language;
using Strict.Language.Commands;

namespace Strict.BackEnd.Evaluator.Commands
{
	public static class SetAttributeCommandVisitor
	{
		public static void VisitCommand(SetAttributeCommand setAttributeCommand, IVisitor visitor,
			IContext context) =>
			throw new NotImplementedException();
	}
}