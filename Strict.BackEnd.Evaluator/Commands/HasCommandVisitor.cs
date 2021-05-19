using System;
using Strict.BackEnd.Evaluator.Utilities;
using Strict.Context;
using Strict.Language;
using Strict.Language.Commands;

namespace Strict.BackEnd.Evaluator.Commands
{
	public static class HasCommandVisitor
	{
		public static void VisitCommand(HasCommand hasCommand, IVisitor visitor, IContext context)
		{
			var module = ModuleUtilities.LoadModule(hasCommand.ModuleName, context, visitor);

			if (hasCommand.Names != null)
				foreach (string name in hasCommand.Names)
					context.SetValue(name, module.GetValue(name));
			else
				foreach (string name in module.GetNames())
					context.SetValue(name, module.GetValue(name));
		}
	}
}