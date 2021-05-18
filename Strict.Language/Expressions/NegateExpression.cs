using System;
using Strict.Context;

namespace Strict.Language.Expressions
{
	public class NegateExpression : IExpression
	{
		private IExpression Expression { get; }

		public NegateExpression(IExpression expression) => Expression = expression;

		public object Accept(IVisitor visitor, IContext context) => throw new NotImplementedException();
	}
}