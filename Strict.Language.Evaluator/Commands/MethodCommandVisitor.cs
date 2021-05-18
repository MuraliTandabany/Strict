using System.Collections.Generic;
using System.Linq;
using Strict.Context;
using Strict.Language;
using Strict.Language.Commands;

namespace Strict.Evaluator.Commands
{
	public static class MethodCommandVisitor
	{
		public static void VisitCommand(MethodCommand methodCommand, IVisitor visitor, IContext context)
		{
			IList<Parameter> parameters = null;
			if (methodCommand.ParameterExpressions is { Count: > 0 })
				parameters = methodCommand.ParameterExpressions.Select(parameterExpression =>
					(Parameter)parameterExpression.Accept(visitor, context)).ToList();
			var function =
				new DefinedFunction(methodCommand.Name, parameters, methodCommand.Body, context);
			function.SetValue("__documentation_", methodCommand.Doc);
			context.SetValue(methodCommand.Name, function);
		}
	}
}