namespace Strict.Language.Expressions
{
	public class ConstantExpression : IExpression
	{
		public object Value { get; }

		public ConstantExpression(object value) => Value = value;
		public object Visitor(IContext context) => null;
	}
}