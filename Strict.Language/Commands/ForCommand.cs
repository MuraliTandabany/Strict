using System;
using Strict.Language.Expressions;

namespace Strict.Language.Commands
{
	public class ForCommand : ICommand
	{
		private string Name { get; }
		public IExpression Expression { get; }
		public ICommand Command { get; }

		public ForCommand(string name, IExpression expression, ICommand command)
		{
			Name = name;
			Expression = expression;
			Command = command;
		}

		public void Visitor(IContext context) => throw new NotImplementedException();
	}
}