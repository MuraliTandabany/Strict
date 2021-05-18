using Strict.Context;
using Strict.Language;

namespace Strict.BackEnd.Evaluator
{
	public interface IType : IValues
	{
		string Name { get; }

		IFunction GetMethod(string name);

		bool HasMethod(string name);
	}
}