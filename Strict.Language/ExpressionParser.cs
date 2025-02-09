﻿namespace Strict.Language;

/// <summary>
/// Abstracts the actual expressions and parsing away to the Expressions project.
/// <see cref="Method.Body"/> will call this lazily when it is called the first time, which is
/// usually not happening until use. This improves performance a lot as we almost do no parsing.
/// </summary>
public abstract class ExpressionParser
{
	public abstract Expression ParseAssignmentExpression(Type type, string initializationLine, int fileLineNumber);
	public abstract Expression ParseMethodBody(Method method);
	public abstract Expression ParseMethodLine(Method.Line line, ref int methodLineNumber);
	public abstract Expression? TryParseExpression(Method.Line line, string remainingPartToParse);
}