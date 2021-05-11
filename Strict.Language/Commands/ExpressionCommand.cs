using Strict.Language.Expressions;

namespace Strict.Language.Commands
{
	public class ExpressionCommand : ICommand
	{
		public IExpression Expression { get; }
		public ExpressionCommand(IExpression expression) => Expression = expression;

		public void Visitor(IContext context) => throw new System.NotImplementedException();
	}
}