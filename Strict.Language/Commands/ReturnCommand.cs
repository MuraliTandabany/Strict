using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
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
