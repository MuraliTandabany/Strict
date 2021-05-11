using System;
using System.Collections.Generic;
using Strict.Language.Expressions;

namespace Strict.Language.Commands
{
	public class ClassCommand : ICommand
	{
		public string Name { get; }
		public ICommand Body { get; }
		public IList<IExpression> BaseExpressions { get; }

		public ClassCommand(string name, ICommand body) : this(name, null, body) { }

		public ClassCommand(string name, IList<IExpression> baseExpressions, ICommand body)
		{
			Name = name;
			Body = body;
			BaseExpressions = baseExpressions;
		}

		public void Visitor(IContext context) => throw new NotImplementedException();
	}
}