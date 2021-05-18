using System.Collections.Generic;
using System.Linq;
using Strict.Context;
using Strict.Language;
using Strict.Language.Commands;
using Strict.Machine;

namespace Strict.BackEnd.Evaluator.Commands
{
	public static class ClassCommandVisitor
	{
		public static void VisitCommand(ClassCommand classCommand, IVisitor visitor, IContext context)
		{
			IList<IType> inheritances = null;
			if (classCommand.InheritancesExpressions is { Count: > 0 })
				inheritances = classCommand.InheritancesExpressions.
					Select(expr => (IType)expr.Accept(visitor, context)).ToList();
			var env = new StrictEnvironment(context);
			var @class = new DefinedClass(classCommand.Name, inheritances, context);
			classCommand.Body.Accept(visitor, @class);
			foreach (var name in env.GetNames())
			{
				var value = env.GetValue(name);
				if (value is DefinedFunction definedFunction)
					@class.SetMethod(definedFunction.Name, definedFunction);
			}
			@class.SetValue("__documentation_", classCommand.Doc);
			context.SetValue(classCommand.Name, @class);
		}
	}
}