using Strict.Context;

namespace Strict.Language.Expressions
{
	public interface IExpression
	{
		object Accept(IVisitor visitor, IContext context);
	}
}