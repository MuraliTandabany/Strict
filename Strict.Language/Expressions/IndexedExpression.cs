using System;
using Strict.Context;

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

		public object Accept(IVisitor visitor, IContext context) => throw new NotImplementedException();
	}
}