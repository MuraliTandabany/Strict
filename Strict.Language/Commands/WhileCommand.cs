using Strict.Context;
using Strict.Language.Expressions;

namespace Strict.Language.Commands
{
	public class WhileCommand : ICommand
	{
		public ICommand Command { get; }
		public IExpression Condition { get; }

		public WhileCommand(IExpression condition, ICommand command)
		{
			Condition = condition;
			Command = command;
		}

		public void Accept(IVisitor visitor, IContext context) => visitor.Visit(this, visitor, context);
	}
}