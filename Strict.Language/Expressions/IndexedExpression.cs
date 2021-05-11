namespace Strict.Language.Expressions
{
	public class IndexedExpression : IExpression
	{
		public IExpression TargetExpression { get; }
		public IExpression IndexExpression { get; }

		public IndexedExpression(IExpression targetExpression, IExpression indexExpression)
		{
			TargetExpression = targetExpression;
			IndexExpression = indexExpression;
		}

		public object Visitor(IContext context) => null;
	}
}