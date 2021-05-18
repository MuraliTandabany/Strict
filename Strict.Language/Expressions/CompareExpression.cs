using System;
using Microsoft.VisualBasic.CompilerServices;
using Strict.Context;

namespace Strict.Language.Expressions
{
	public class CompareExpression : BinaryExpression
	{
		public ComparisonOperator Operation { get; }
		public Func<object, object, bool, object> Function { get; private set; }

		public CompareExpression(ComparisonOperator operation, IExpression left, IExpression right) :
			base(left, right)
		{
			Operation = operation;
			Function = operation switch
			{
				ComparisonOperator.Equal => Operators.CompareObjectEqual,
				ComparisonOperator.NotEqual => Operators.CompareObjectNotEqual,
				ComparisonOperator.Less => Operators.CompareObjectLess,
				ComparisonOperator.LessEqual => Operators.CompareObjectLessEqual,
				ComparisonOperator.Greater => Operators.CompareObjectGreater,
				ComparisonOperator.GreaterEqual => Operators.CompareObjectGreaterEqual,
				_ => Function
			};
		}

		public override object Accept(IVisitor visitor, IContext context) => null;
	}
}