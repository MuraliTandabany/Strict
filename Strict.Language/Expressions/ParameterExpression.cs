using System;
using Strict.Context;

namespace Strict.Language.Expressions
{
	public class ParameterExpression : IExpression
	{
		public string Name { get; }
		public IExpression DefaultExpression { get; }
		public bool IsList { get; }

		public ParameterExpression(string name, IExpression defaultExpression, bool isList)
		{
			Name = name;
			DefaultExpression = defaultExpression;
			IsList = isList;
		}

		public object Accept(IVisitor visitor, IContext context) => throw new NotImplementedException();
	}
}