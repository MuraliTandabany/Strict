using System.Collections.Generic;
using Strict.Context;

namespace Strict.Language.Commands
{
	public class CompositeCommand : ICommand
	{
		public ICollection<ICommand> Commands { get; }

		public CompositeCommand() => Commands = new List<ICommand>();
		public CompositeCommand(IList<ICommand> commands) => Commands = commands;
		public void AddCommand(ICommand command) => Commands.Add(command);

		public void Accept(IVisitor visitor, IContext context) => visitor.Visit(this, visitor, context);
	}
}