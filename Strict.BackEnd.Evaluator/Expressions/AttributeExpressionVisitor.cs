using System;
using Strict.Context;
using Strict.Language;
using Strict.Language.Expressions;

namespace Strict.BackEnd.Evaluator.Expressions
{
	public static class AttributeExpressionVisitor
	{
		public static object VisitExpression(AttributeExpression attributeExpression, IVisitor visitor,
			IContext context) =>
			throw new NotImplementedException();
	}
}