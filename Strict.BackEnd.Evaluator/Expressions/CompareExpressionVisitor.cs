using Strict.Context;
using Strict.Language;
using Strict.Language.Expressions;

namespace Strict.BackEnd.Evaluator.Expressions
{
	public static class CompareExpressionVisitor
	{
		public static object VisitExpression(CompareExpression compareExpression, IVisitor visitor,
			IContext context)
		{
			var leftValue = compareExpression.Left.Accept(visitor, context);
			var rightValue = compareExpression.Right.Accept(visitor, context);
			return compareExpression.Function(leftValue, rightValue, false);
		}
	}
}