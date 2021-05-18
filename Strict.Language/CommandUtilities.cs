using Strict.Language.Commands;

namespace Strict.Language
{
	public static class CommandUtilities
	{
		public static string GetDocString(ICommand command)
		{
			var composite = command as CompositeCommand;
			return composite != null
				? composite.GetDocString()
				: null;
		}
	}
}