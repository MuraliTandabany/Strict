using System;
using Strict.Context;

namespace Strict.Language.Expressions
{
	public class AttributeExpression : IExpression
	{
		public string Name { get; }
		public IExpression Expression { get; }

		public AttributeExpression(IExpression expression, string name)
		{
			Expression = expression;
			Name = name;
		}

		public object Accept(IVisitor visitor, IContext context) => throw new NotImplementedException();
	}
}