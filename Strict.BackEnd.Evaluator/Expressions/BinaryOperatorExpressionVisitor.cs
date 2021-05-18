using System;
using Strict.Context;
using Strict.Language;
using Strict.Language.Expressions;

namespace Strict.BackEnd.Evaluator.Expressions
{
	public static class BinaryOperatorExpressionVisitor
	{
		public static object VisitExpression(BinaryOperatorExpression binaryOperatorExpression,
			IVisitor visitor, IContext context)
		{
			object leftValue;
			object rightValue;
			leftValue = binaryOperatorExpression.Left.Accept(visitor, context);
			rightValue = binaryOperatorExpression.Right.Accept(visitor, context);
			return binaryOperatorExpression.Operator switch
			{
				BinaryOperator.Add => Numbers.Add(leftValue, rightValue),
				BinaryOperator.Subtract => Numbers.Subtract(leftValue, rightValue),
				BinaryOperator.Multiply => Strings.IsString(leftValue)
					? Strings.Multiply(leftValue, rightValue)
					: Numbers.Multiply(leftValue, rightValue),
				BinaryOperator.Divide => Numbers.Divide(leftValue, rightValue),
				BinaryOperator.Power => Convert.ToInt32(Math.Pow((int)leftValue, (int)rightValue)),
				_ => throw new InvalidOperationException(),
			};
		}
	}
}