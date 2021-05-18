using Strict.Context;
using Strict.Language.Expressions;

namespace Strict.Language.Commands
{
	public class ReturnCommand : ICommand
	{
		public IExpression Expression { get; }

		public ReturnCommand(IExpression expression) => Expression = expression;

		public void Accept(IVisitor visitor, IContext context) => visitor.Visit(this, visitor, context);
	}
}