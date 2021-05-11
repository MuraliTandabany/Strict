namespace Strict.Language.Expressions
{
	public interface IExpression
	{
		object Visitor(IContext context);
	}
}