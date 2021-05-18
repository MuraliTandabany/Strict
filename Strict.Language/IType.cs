using Strict.Context;

namespace Strict.Language
{
	public interface IType : IValues
	{
		string Name { get; }

		IFunction GetMethod(string name);

		bool HasMethod(string name);
	}
}