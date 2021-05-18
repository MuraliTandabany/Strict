using Strict.Context;
using Strict.Language;
using Strict.Language.Expressions;

namespace Strict.Evaluator.Expressions
{
	public static class BooleanExpressionVisitor
	{
		public static object VisitExpression(BooleanExpression booleanExpression, IVisitor visitor,
			IContext context)
		{
			object leftValue;
			object rightValue;
			leftValue = booleanExpression.Left.Accept(visitor, context);
			if (booleanExpression.Operation != BooleanOperator.Or)
			{
				if (Predicates.IsFalse(leftValue))
					return false;
			}
			else
			{
				if (!Predicates.IsFalse(leftValue))
					return true;
			}
			rightValue = booleanExpression.Right.Accept(visitor, context);
			return !Predicates.IsFalse(rightValue);
		}
	}
}