using Strict.Context;
using Strict.Language.Expressions;

namespace Strict.Language.Commands
{
	public class ForCommand : ICommand
	{
		public string Name { get; }
		public IExpression Expression { get; }
		public ICommand Command { get; }

		public ForCommand(string name, IExpression expression, ICommand command)
		{
			Name = name;
			Expression = expression;
			Command = command;
		}

		public void Accept(IVisitor visitor, IContext context) => visitor.Visit(this, visitor, context);
	}
}