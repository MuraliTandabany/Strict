using Strict.Context;
using Strict.Language.Expressions;

namespace Strict.Language.Commands
{
	public class IfCommand : ICommand
	{
		public IExpression Condition { get; }
		public ICommand ThenCommand { get; }
		public ICommand ElseCommand { get; }

		public IfCommand(IExpression condition, ICommand thenCommand) : this(condition, thenCommand,
			null) { }

		public IfCommand(IExpression condition, ICommand thenCommand, ICommand elseCommand)
		{
			Condition = condition;
			ThenCommand = thenCommand;
			ElseCommand = elseCommand;
		}

		public void Accept(IVisitor visitor, IContext context) => visitor.Visit(this, visitor, context);
	}
}