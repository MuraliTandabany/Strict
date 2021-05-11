﻿using System.Collections.Generic;
using System.Linq;
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
			if (ParameterExpressions == null)
				return;
			if (ParameterExpressions.Any(x => x.DefaultExpression != null))
				throw new SyntaxErrorException("non-default argument follows default argument");
		}

		public void Visitor(IContext context) => throw new System.NotImplementedException();
	}
}