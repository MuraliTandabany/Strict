﻿using System;
using System.Collections.Generic;
using System.Linq;

namespace Strict.Language.Expressions;

// ReSharper disable once HollowTypeName
public class MethodCall : Expression
{
	public MethodCall(Expression? instance, Method method, params Expression[] arguments) : base(
		method.ReturnType)
	{
		Instance = instance;
		Method = method;
		Arguments = arguments;
	}

	public Expression? Instance { get; }
	public Method Method { get; }
	public IReadOnlyList<Expression> Arguments { get; }

	public override string ToString() =>
		(Instance != null
			? Instance + "."
			: "") + Method.Name + Arguments.ToBrackets();

	public static Expression? TryParse(Method.Line line, string partToParse) =>
		partToParse.EndsWith(')') && partToParse.Contains('(')
			? TryParseMethod(line,
				partToParse.Split(new[] { '(', ')' }, StringSplitOptions.RemoveEmptyEntries))
			: TryParseMethod(line, partToParse);

	private static Expression? TryParseMethod(Method.Line line, params string[] parts)
	{
		var methodName = parts[0];
		var arguments = parts.Length > 1
			? GetArguments(line, parts[1], methodName)
			: Array.Empty<Expression>();
		if (!methodName.Contains('.'))
			return FindMethodCall(null, line, methodName, arguments);
		var memberParts = methodName.Split('.', 2);
		var firstMember = MemberCall.TryParse(line, memberParts[0]);
		if (firstMember == null)
			throw new MemberCall.MemberNotFound(line, line.Method.Type, memberParts[0]);
		return FindMethodCall(firstMember, line, memberParts[1], arguments);
	}

	private static Expression[] GetArguments(Method.Line line, string argumentsText, string methodName)
	{
		var parts = argumentsText.Split(", ");
		var arguments = new Expression[parts.Length];
		for (var index = 0; index < parts.Length; index++)
			arguments[index] = line.Method.TryParseExpression(line, parts[index]) ??
				throw new InvalidExpressionForArgument(line,
					parts[index] + " for " + methodName + " argument " + index);
		return arguments;
	}

	public sealed class InvalidExpressionForArgument : ParsingFailed
	{
		public InvalidExpressionForArgument(Method.Line line, string message) : base(line, message) { }
	}

	// ReSharper disable once TooManyArguments
	private static MethodCall? FindMethodCall(Expression? instance, Method.Line line, string methodName,
		Expression[] arguments)
	{
		if (!methodName.IsWord())
			return null;
		var type = instance?.ReturnType ?? line.Method.Type;
		if (methodName == type.Name)
			methodName = Method.From;
		foreach (var method in type.Methods)
			if (method.Name == methodName && method.Parameters.Count == arguments.Length)
				return new MethodCall(instance, method, arguments);
		return type.Methods.All(m => m.Name != methodName)
			? throw new MethodNotFound(line, methodName, type)
			: throw new ArgumentsDoNotMatchMethodParameters(line, arguments,
				type.Methods.Where(n => n.Name == methodName));
	}

	public sealed class MethodNotFound : ParsingFailed
	{
		public MethodNotFound(Method.Line line, string methodName, Type referencingType) : base(line, methodName, referencingType) { }
	}

	public sealed class ArgumentsDoNotMatchMethodParameters : ParsingFailed
	{
		public ArgumentsDoNotMatchMethodParameters(Method.Line line, Expression[] arguments,
			IEnumerable<Method> methods) : base(line, (arguments.Length == 0
			? "No arguments does "
			: "Arguments: " + arguments.ToBrackets() + " do ") + "not match " + methods.ToWordList()) { }
	}
}