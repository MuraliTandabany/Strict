using System;
using Strict.Context;

namespace Strict.Language.Expressions
{
	public class NotExpression : IExpression
	{
		public IExpression Expression { get; }

		public NotExpression(IExpression expression) => Expression = expression;

		public object Accept(IVisitor visitor, IContext context) => throw new NotImplementedException();
	}
}