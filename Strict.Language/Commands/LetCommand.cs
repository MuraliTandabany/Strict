using System;
using Strict.Context;
using Strict.Language.Expressions;

namespace Strict.Language.Commands
{
	public class LetCommand : ICommand
	{
		public string Target { get; }

		public IExpression Expression { get; }

		public LetCommand(string target, IExpression expression)
		{
			Target = target ?? throw new ArgumentNullException(nameof(target));
			Expression = expression ?? throw new ArgumentNullException(nameof(expression));
		}

		public void Accept(IVisitor visitor, IContext context) => visitor.Visit(this, visitor, context);
	}
}