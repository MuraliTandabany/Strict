using System;
using System.Collections.Generic;

namespace Strict.Language.Commands
{
	public class CompositeCommand : ICommand
	{
		public ICollection<ICommand> Commands { get; }

		public CompositeCommand() => Commands = new List<ICommand>();
		public CompositeCommand(IList<ICommand> commands) => Commands = commands;
		public void AddCommand(ICommand command) => Commands.Add(command);

		public void Visitor(IContext context) => throw new NotImplementedException();
	}
}