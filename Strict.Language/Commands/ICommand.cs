namespace Strict.Language.Commands
{
	public interface ICommand
	{
		void Visitor(IContext context);
	}
}