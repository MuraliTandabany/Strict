using System;
using System.Collections.Generic;
using Strict.Context;

namespace Strict.Language.Expressions
{
	public class ListExpression : IExpression
	{
		private IList<IExpression> Expressions { get; }
		public bool IsReadonly { get; }

		public ListExpression(IList<IExpression> expressions) : this(expressions, false) { }

		public ListExpression(IList<IExpression> expressions, bool isReadonly)
		{
			Expressions = expressions;
			IsReadonly = isReadonly;
		}

		public object Accept(IVisitor visitor, IContext context) => throw new NotImplementedException();
	}
}