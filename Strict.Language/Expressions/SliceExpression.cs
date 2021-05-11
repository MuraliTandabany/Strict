namespace Strict.Language.Expressions
{
	public class SliceExpression : IExpression
	{
		public IExpression BeginExpression { get; }
		public IExpression EndExpression { get; }

		public SliceExpression(IExpression beginExpression, IExpression endExpression)
		{
			BeginExpression = beginExpression;
			EndExpression = endExpression;
		}

		public object Visitor(IContext context) => null;
	}
}