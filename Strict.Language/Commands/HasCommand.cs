using System.Collections.Generic;

namespace Strict.Language.Commands
{
	public class HasCommand : ICommand
	{
		public string ModuleName { get; }
		public ICollection<string> Names { get; }

		public HasCommand(string name) : this(name, null) { }

		public HasCommand(string name, IList<string> names)
		{
			ModuleName = name;
			Names = names;
		}

		public void Visitor(IContext context) => throw new System.NotImplementedException();
	}
}