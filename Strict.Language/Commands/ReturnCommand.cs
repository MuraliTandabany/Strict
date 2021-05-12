using System;
using Strict.Language.Expressions;

namespace Strict.Language.Commands
{
	public class ReturnCommand : ICommand
	{
		public IExpression Expression { get; }

		public ReturnCommand(IExpression expression) => Expression = expression;

		public void Visitor(IContext context) => throw new NotImplementedException();
	}
}