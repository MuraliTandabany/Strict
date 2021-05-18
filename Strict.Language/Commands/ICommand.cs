using Strict.Context;

namespace Strict.Language.Commands
{
	public interface ICommand
	{
		void Accept(IVisitor visitor, IContext context);
	}
}