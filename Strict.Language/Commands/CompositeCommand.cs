using System.Collections.Generic;
using System.Linq;
using Strict.Context;
using Strict.Language.Expressions;

namespace Strict.Language.Commands
{
	public class CompositeCommand : ICommand
	{
		public IList<ICommand> Commands { get; }

		public CompositeCommand() => Commands = new List<ICommand>();
		public CompositeCommand(IList<ICommand> commands) => Commands = commands;
		public void AddCommand(ICommand command) => Commands.Add(command);

		public string GetDocString()
		{
			if (Commands == null || Commands.Count == 0)
				return null;
			var first = Commands.First();
			if (first is not ExpressionCommand { Expression: ConstantExpression } expressionCommand)
				return null;
			var constantExpression = (ConstantExpression)expressionCommand.Expression;
			if (constantExpression.Value is not string str)
				return null;
			Commands.RemoveAt(0);
			return str;
		}

		public void Accept(IVisitor visitor, IContext context) => visitor.Visit(this, visitor, context);
	}
}