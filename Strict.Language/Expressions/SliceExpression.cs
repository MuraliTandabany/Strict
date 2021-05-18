using System;
using Strict.Context;

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

		public object Accept(IVisitor visitor, IContext context) => throw new NotImplementedException();
	}
}