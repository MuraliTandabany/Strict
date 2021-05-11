namespace Strict.Language.Expressions
{
	public class BooleanExpression : BinaryExpression
	{
		private BooleanOperator Operation { get; }

		public BooleanExpression(IExpression left, IExpression right, BooleanOperator operation) :
			base(left, right) =>
			Operation = operation;
	}
}