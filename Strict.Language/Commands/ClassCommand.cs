using System.Collections.Generic;
using Strict.Context;
using Strict.Language.Expressions;

namespace Strict.Language.Commands
{
	public class ClassCommand : ICommand
	{
		public string Name { get; }
		public ICommand Body { get; }
		public IList<IExpression> InheritancesExpressions { get; }
		public string Doc { get; }

		public ClassCommand(string name, ICommand body) : this(name, null, body) { }

		public ClassCommand(string name, IList<IExpression> inheritancesExpressions, ICommand body)
		{
			Name = name;
			Body = body;
			InheritancesExpressions = inheritancesExpressions;
			Doc = CommandUtilities.GetDocString(Body);
		}

		public void Accept(IVisitor visitor, IContext context) => visitor.Visit(this, visitor, context);
	}
}