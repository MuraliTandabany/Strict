using Strict.Context;
using Strict.Exceptions;
using Strict.Language;
using Strict.Language.Expressions;

namespace Strict.Evaluator.Expressions
{
	public static class NameExpressionVisitor
	{
		public static object VisitExpression(NameExpression nameExpression, IVisitor visitor,
			IContext context)
		{
			var value = context.GetValue(nameExpression.Name) ??
				context.GlobalContext.GetValue(nameExpression.Name);
			return value ?? (context.HasValue(nameExpression.Name) ||
				context.GlobalContext.HasValue(nameExpression.Name)
					? value
					: throw new NameErrorException($"name '{nameExpression.Name}' is not defined"));
		}
	}
}