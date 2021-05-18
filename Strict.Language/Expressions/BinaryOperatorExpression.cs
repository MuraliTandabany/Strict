using Strict.Context;

namespace Strict.Language.Expressions
{
	public class BinaryOperatorExpression : BinaryExpression
	{
		public BinaryOperator Operator { get; }

		public BinaryOperatorExpression(IExpression left, IExpression right,
			BinaryOperator binaryOperator) : base(left, right) =>
			Operator = binaryOperator;

		public override object Accept(IVisitor visitor, IContext context) =>
			visitor.Visit(this, visitor, context);
	}
}