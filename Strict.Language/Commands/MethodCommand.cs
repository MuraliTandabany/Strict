using System.Collections.Generic;
using Strict.Context;
using Strict.Exceptions;
using Strict.Language.Expressions;

namespace Strict.Language.Commands
{
	public class MethodCommand : ICommand
	{
		public string Name { get; }
		public IList<ParameterExpression> ParameterExpressions { get; }
		public ICommand Body { get; }
		public string Doc { get; }

		public MethodCommand(string name, IList<ParameterExpression> parameterExpressions,
			ICommand body)
		{
			Name = name;
			ParameterExpressions = parameterExpressions;
			Body = body;
			Doc = CommandUtilities.GetDocString(Body);
			if (ParameterExpressions == null)
				return;
			var hasDefault = false;
			foreach (var parameterExpression in ParameterExpressions)
				if (parameterExpression.DefaultExpression != null)
					hasDefault = true;
				else if (hasDefault)
					throw new SyntaxErrorException("non-default argument follows default argument");
		}

		public void Accept(IVisitor visitor, IContext context) => visitor.Visit(this, visitor, context);
	}
}