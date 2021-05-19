using System;
using System.Collections.Generic;
using System.Linq;
using Strict.BackEnd.Evaluator.Utilities;
using Strict.Context;
using Strict.Language;
using Strict.Language.Expressions;

namespace Strict.BackEnd.Evaluator.Expressions
{
	public static class CallExpressionVisitor
	{
		public static object VisitExpression(CallExpression callExpression, IVisitor visitor,
			IContext context)
		{
			IList<object> arguments = null;
			IDictionary<string, object> namedArguments = null;
			if (callExpression.HasNames)
				namedArguments = new Dictionary<string, object>();
			if (callExpression.ArgumentExpressions != null &&
				callExpression.ArgumentExpressions.Count > 0)
			{
				arguments = new List<object>();
				foreach (var argumentExpr in callExpression.ArgumentExpressions)
				{
					var value = argumentExpr.Accept(visitor, context);
					if (callExpression.HasNames && argumentExpr is NamedArgumentExpression)
						namedArguments[((NamedArgumentExpression)argumentExpr).Name] = value;
					else
						arguments.Add(argumentExpr.Accept(visitor, context));
				}
			}
			IFunction function = null;

			// TODO to skip AttributeExpression, or have a separated MethodCallExpression 
			if (callExpression.IsObject)
			{
				var attrexpr = (AttributeExpression)callExpression.TargetExpression;
				var obj = attrexpr.Expression.Accept(visitor, context);
				if (obj is DynamicObject)
				{
					var dynobj = (DynamicObject)obj;
					return dynobj.Invoke(attrexpr.Name, context, arguments, namedArguments);
				}
				function = GetFunction(obj, attrexpr.Name);
				if (function != null)
					return function.Visit(context, arguments, namedArguments);
				var type = Types.GetType(obj);
				if (type == null)
				{
					var values = obj as IValues;
					if (values != null && values.HasValue(attrexpr.Name))
					{
						var value = values.GetValue(attrexpr.Name);
						function = value as IFunction;
						if (function == null && value is Type)
							return Activator.CreateInstance((Type)value, arguments == null
								? null
								: arguments.ToArray());
					}
					if (function == null)
					{
						if (obj is Type)
							return TypeUtilities.InvokeTypeMember((Type)obj, attrexpr.Name, arguments);
						return ObjectUtilities.GetValue(obj, attrexpr.Name, arguments);
					}
				}
				function = type.GetMethod(attrexpr.Name);
				arguments.Insert(0, obj);
			}
			else
			{
				var value = callExpression.TargetExpression.Accept(visitor, context);
				function = value as IFunction;
				return function == null
					? Activator.CreateInstance((Type)value, arguments?.ToArray())
					: function.Visit(context, arguments, namedArguments);
			}
			return function.Visit(context, arguments, namedArguments);
		}

		private static IFunction GetFunction(object obj, string name)
		{
			IFunction function = null;
			if (obj is IType)
			{
				function = ((IType)obj).GetMethod(name);
				if (function != null)
					return function;
			}
			if (obj is not IValues)
				return null;
			function = ((IValues)obj).GetValue(name) as IFunction;
			return function ?? null;
		}
	}
}