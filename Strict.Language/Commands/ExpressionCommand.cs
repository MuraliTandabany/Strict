using Strict.Language.Expressions;

namespace Strict.Language.Commands
{
    public class ExpressionCommand : ICommand
    {
        public IExpression Expression { get; }
        public ExpressionCommand(IExpression expression) => Expression = expression;
    }
}