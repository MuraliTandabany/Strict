using System;
using Strict.Context;

namespace Strict.Language.Expressions
{
	public class NamedArgumentExpression : IExpression
	{
		public string Name { get; }
		public IExpression Expression { get; }

		public NamedArgumentExpression(string name, IExpression expression)
		{
			Name = name;
			Expression = expression;
		}

		public object Accept(IVisitor visitor, IContext context) => throw new NotImplementedException();
	}
}