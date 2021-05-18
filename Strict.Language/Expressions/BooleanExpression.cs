using Strict.Context;

namespace Strict.Language.Expressions
{
	public class BooleanExpression : BinaryExpression
	{
		public BooleanOperator Operation { get; }

		public BooleanExpression(IExpression left, IExpression right, BooleanOperator operation) :
			base(left, right) =>
			Operation = operation;

		public override object Accept(IVisitor visitor, IContext context) => null;
	}
}