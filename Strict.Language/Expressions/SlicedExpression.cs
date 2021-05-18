using System;
using Strict.Context;

namespace Strict.Language.Expressions
{
	public class SlicedExpression : IExpression
	{
		public IExpression TargetExpression { get; }
		public SliceExpression SliceExpression { get; }

		public SlicedExpression(IExpression targetExpression, SliceExpression sliceExpression)
		{
			TargetExpression = targetExpression;
			SliceExpression = sliceExpression;
		}

		public object Accept(IVisitor visitor, IContext context) => throw new NotImplementedException();
	}
}