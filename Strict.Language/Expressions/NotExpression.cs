namespace Strict.Language.Expressions
{
    public class NotExpression : IExpression
    {
        public IExpression Expression { get; }

        public NotExpression(IExpression expression) => Expression = expression;
    }
}