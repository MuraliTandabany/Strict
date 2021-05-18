using Strict.Language;

namespace Strict.Machine
{
	public interface IStrictMachine
	{
		IVisitor Visitor { get; }
		StrictEnvironment Environment { get; set; }
	}
}