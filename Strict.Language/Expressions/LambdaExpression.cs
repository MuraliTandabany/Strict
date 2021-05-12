using System;
using System.Collections.Generic;

namespace Strict.Language.Expressions
{
	public class LambdaExpression : IExpression
	{
		public IExpression TargetExpression { get; }
		public IList<IExpression> ArgumentExpressions { get; }

		public LambdaExpression(IExpression targetExpression, IList<IExpression> argumentExpressions)
		{
			TargetExpression = targetExpression;
			ArgumentExpressions = argumentExpressions;
		}

		public object Visitor(IContext context) => throw new NotImplementedException();
	}
}