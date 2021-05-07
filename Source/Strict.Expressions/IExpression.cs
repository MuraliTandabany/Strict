using Strict.Language;

namespace Strict.Expressions
{
    public interface IExpression
    {
        object Evaluate(IContext context);
    }
}