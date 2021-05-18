using System;
using System.Collections;
using System.Linq;
using Strict.Context;
using Strict.Language;
using Strict.Language.Commands;
using Strict.Machine;

namespace Strict.BackEnd.Evaluator.Commands
{
	public static class ForCommandVisitor
	{
		public static void VisitCommand(ForCommand forCommand, IVisitor visitor, IContext context)
		{
			var environment = context as StrictEnvironment;
			var value = forCommand.Expression.Accept(visitor, context);
			var items = value as IEnumerable ?? Enumerable.Range(0, Convert.ToInt32(value));
			foreach (var item in items)
			{
				context.SetValue(forCommand.Name, item);
				forCommand.Command.Accept(visitor, context);
				if (environment == null)
					continue;
				if (environment.HasReturnValue)
					return;
				if (environment.WasBreak)
				{
					environment.WasBreak = false;
					break;
				}
				if (environment.WasContinue)
					environment.WasContinue = false;
			}
		}
	}
}