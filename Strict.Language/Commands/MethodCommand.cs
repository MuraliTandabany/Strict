using System;
using System.Collections.Generic;
using Strict.Exceptions;
using Strict.Language.Expressions;

namespace Strict.Language.Commands
{
	public class MethodCommand : ICommand
	{
		public string Name { get; }

		public IList<ParameterExpression> ParameterExpressions { get; }

		public ICommand Body { get; }

		public MethodCommand(string name, IList<ParameterExpression> parameterExpressions,
			ICommand body)
		{
			Name = name;
			ParameterExpressions = parameterExpressions;
			Body = body;
			if (ParameterExpressions != null)
			{
				var hasDefault = false;
				foreach (var parameterExpression in ParameterExpressions)
					if (parameterExpression.DefaultExpression != null)
						hasDefault = true;
					else if (hasDefault)
						throw new SyntaxErrorException("non-default argument follows default argument");
			}
		}

		public void Visitor(IContext context) => throw new NotImplementedException();
	}
}