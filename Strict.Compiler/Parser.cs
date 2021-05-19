using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using Strict.Compiler.Exceptions;
using Strict.Exceptions;
using Strict.Language;
using Strict.Language.Commands;
using Strict.Language.Expressions;

namespace Strict.Compiler
{
	public class Parser
	{
		private static HashSet<string> Opslevel0 { get; } = new()
		{
			">",
			"<",
			">=",
			"<=",
			"<>",
			"==",
			"!=",
			"|",
			"&"
		};
		private static HashSet<string> Opslevel1 { get; } = new() { "+", "-" };
		private static HashSet<string> Opslevel2 { get; } = new() { "*", "/" };
		private static HashSet<string> Opslevel3 { get; } = new()
		{
			"**",
			"++",
			"--",
			"|>",
			"|<",
			"=>"
		};
		private readonly Dictionary<string, Func<Parser, ICommand>> commandActions = new()
		{
			{ "has", p => p.CompileHasCommand() },
			{ "if", p => p.CompileIfCommand() },
			{ "implement", p => p.CompileClassCommand() },
			{ "for", p => p.CompileForCommand() },
			{ "while", p => p.CompileWhileCommand() },
			{ "method", p => p.CompileMethodCommand() },
			{ "let", p => p.CompileLetCommand() },
			{ "return", p => p.CompileReturnCommand() }
		};

		private int IndentLevel { get; set; }
		private Lexer LexicalAnalyzer { get; }

		public Parser(Lexer lexer) =>
			LexicalAnalyzer = lexer ?? throw new ArgumentNullException(nameof(lexer));

		public Parser(string text) : this(new Lexer(text)) { }

		public Parser(TextReader reader) : this(new Lexer(reader)) { }

		private static bool IsLevel0Operator(Token token) =>
			token is { TokenType: TokenType.Operator } && Opslevel0.Contains(token.Value);

		private static bool IsLevel1Operator(Token token) =>
			token is { TokenType: TokenType.Operator } && Opslevel1.Contains(token.Value);

		private static bool IsLevel2Operator(Token token) =>
			token is { TokenType: TokenType.Operator } && Opslevel2.Contains(token.Value);

		private static bool IsLevel3Operator(Token token) =>
			token is { TokenType: TokenType.Operator } && Opslevel3.Contains(token.Value);

		public IExpression CompileExpression() => CompileOrExpression();

		public IList<IExpression> CompileExpressionList()
		{
			IList<IExpression> expressions = new List<IExpression>();
			var expression = CompileExpression();
			if (expression == null)
				return null;
			expressions.Add(expression);
			while (TryCompile(TokenType.Separator, ","))
				expressions.Add(CompileExpression());
			return expressions;
		}

		public IExpression CompileList()
		{
			var list = CompileExpressionList();
			return list == null
				? new ListExpression(new List<IExpression>())
				: new ListExpression(list);
		}

		public ICommand CompileCommand()
		{
			SkipEmptyLines();
			var newIndentation = LexicalAnalyzer.NextIndent();
			if (newIndentation < IndentLevel)
			{
				SkipEmptyLines();
				LexicalAnalyzer.PushIndent(newIndentation);
				return null;
			}
			if (newIndentation > IndentLevel)
				throw new SyntaxErrorException("unexpected indent");
			return CompileSimpleCommand();
		}

		private List<ICommand> ProcessCommands()
		{
			var commands = new List<ICommand>();
			while (true)
			{
				var command = CompileCommand();
				if (command == null)
					break;
				commands.Add(command);
			}
			return commands;
		}

		public ICommand CompileCommandList()
		{
			var commands = ProcessCommands();
			return commands.Count switch
			{
				0 => null,
				1 => commands[0],
				_ => new CompositeCommand(commands)
			};
		}

		public ICommand CompileNestedCommandList(int newIndentation)
		{
			var oldIndentation = IndentLevel;
			try
			{
				IndentLevel = newIndentation;
				LexicalAnalyzer.PushIndent(newIndentation);
				var commands = ProcessCommands();
				return commands.Count switch
				{
					0 => null,
					1 => commands[0],
					_ => new CompositeCommand(commands)
				};
			}
			finally
			{
				IndentLevel = oldIndentation;
			}
		}

		private static BinaryOperator CompileOperator(string oper) =>
			oper switch
			{
				"+" => BinaryOperator.Add,
				"-" => BinaryOperator.Subtract,
				"*" => BinaryOperator.Multiply,
				"/" => BinaryOperator.Divide,
				"**" => BinaryOperator.Power,
				"++" => BinaryOperator.Increment,
				"--" => BinaryOperator.Decrement,
				_ => throw new InvalidOperationException($"Unexpected {oper}")
			};

		private static ComparisonOperator CompileCompareOperator(string oper) =>
			oper switch
			{
				"<" => ComparisonOperator.Less,
				"<=" => ComparisonOperator.LessEqual,
				">" => ComparisonOperator.Greater,
				">=" => ComparisonOperator.GreaterEqual,
				"==" => ComparisonOperator.Equal,
				"<>" => ComparisonOperator.NotEqual,
				"!=" => ComparisonOperator.NotEqual,
				_ => throw new InvalidOperationException($"Unexpected {oper}")
			};

		private void CompileEndOfCommand()
		{
			var token = LexicalAnalyzer.NextToken();
			if (token == null)
				return;
			if (token.TokenType == TokenType.EndOfLine)
				return;
			if (token.TokenType != TokenType.Separator)
				throw new UnexpectedTokenException(token.Value);
		}

		private bool TryPeekCompileEndOfCommand()
		{
			var token = LexicalAnalyzer.NextToken();
			LexicalAnalyzer.PushToken(token);
			return token == null || token.TokenType == TokenType.EndOfLine;
		}

		private ExpressionCommand CompileExpressionCommand()
		{
			var list = CompileExpressionList();
			if (list == null)
				return null;
			return list.Count == 1
				? new ExpressionCommand(list[0])
				: new ExpressionCommand(new ListExpression(list, true));
		}

		private ICommand CompileSimpleCommand()
		{
			var token = TryCompile(TokenType.Name);
			if (token == null)
			{
				ICommand command = CompileExpressionCommand();
				CompileEndOfCommand();
				return command;
			}
			if (commandActions.ContainsKey(token.Value))
				return commandActions[token.Value](this);
			LexicalAnalyzer.PushToken(token);
			var expressionCommand = CompileExpressionCommand();
			if (!TryCompile(TokenType.Operator, "="))
			{
				CompileEndOfCommand();
				return expressionCommand;
			}
			var valueExpression = CompileExpression();
			return expressionCommand.Expression switch
			{
				NameExpression => CompileNameExpression(expressionCommand, valueExpression),
				AttributeExpression => CompileAttributeExpression(expressionCommand, valueExpression),
				_ => throw new SyntaxErrorException("invalid assignment")
			};
		}

		private ICommand CompileNameExpression(ExpressionCommand expressionCommand,
			IExpression valueExpression)
		{
			var command = new LetCommand(((NameExpression)expressionCommand.Expression).Name,
				valueExpression);
			CompileEndOfCommand();
			return command;
		}

		private ICommand CompileAttributeExpression(ExpressionCommand expressionCommand,
			IExpression valueExpression)
		{
			var command = new SetAttributeCommand(
				((AttributeExpression)expressionCommand.Expression).Expression,
				((AttributeExpression)expressionCommand.Expression).Name, valueExpression);
			CompileEndOfCommand();
			return command;
		}

		private IList<string> CompileNameList()
		{
			var names = new List<string>();
			names.Add(CompileName(true).Value);
			while (TryCompile(TokenType.Separator, ","))
			{
				var name = CompileName(true).Value;
				while (TryCompile(TokenType.Operator, "."))
					name += "." + CompileName(true).Value;
				names.Add(name);
			}
			return names;
		}

		private IList<ParameterExpression> CompileParameterExpressionList()
		{
			var parameters = new List<ParameterExpression>();
			if (TryPeekCompile(TokenType.Separator, ")"))
				return parameters;
			var parameterExpression = CompileParameterExpression();
			var hasList = parameterExpression.IsList;
			parameters.Add(parameterExpression);
			while (TryCompile(TokenType.Separator, ","))
			{
				parameterExpression = CompileParameterExpression();
				if (parameterExpression.IsList)
					hasList = hasList
						? throw new SyntaxErrorException("invalid syntax")
						: true;
				parameters.Add(parameterExpression);
			}
			return parameters;
		}

		private ParameterExpression CompileParameterExpression()
		{
			var isList = TryCompile(TokenType.Operator, "*");
			var name = CompileName(true).Value;
			IExpression expression = null;
			if (TryCompile(TokenType.Operator, "="))
				expression = CompileExpression();
			return new ParameterExpression(name, expression, isList);
		}

		private ICommand CompileSuite() => CompileSuite(true);

		private ICommand CompileSuite(bool checkForNewIndentation)
		{
			var token = LexicalAnalyzer.NextToken();
			if (token == null)
				throw new UnexpectedEndOfInputException();
			if (token.TokenType == TokenType.EndOfLine)
				return CompileNestedCommandList(checkForNewIndentation
					? LexicalAnalyzer.NextIndent()
					: IndentLevel);
			LexicalAnalyzer.PushToken(token);
			return CompileCommandList();
		}

		private ICommand CompileForCommand()
		{
			var name = CompileName(true).Value;
			CompileToken(TokenType.Name, "to");
			var expression = CompileExpression();
			var command = CompileSuite();
			return new ForCommand(name, expression, command);
		}

		private ICommand CompileHasCommand()
		{
			var name = CompileName(true).Value;
			while (TryCompile(TokenType.Operator, "."))
				name += "." + CompileName(true).Value;
			var token = LexicalAnalyzer.NextToken();
			if (token == null || token.TokenType == TokenType.EndOfLine)
				return new HasCommand(name);
			var names = CompileNameList();
			CompileEndOfCommand();
			return new HasCommand(name, names);
		}

		private ICommand CompileIfCommand()
		{
			var condition = CompileExpression();
			ICommand thencommand;
			thencommand = CompileSuite();
			var indent = LexicalAnalyzer.NextIndent();
			if (indent == IndentLevel)
			{
				if (TryCompile(TokenType.Name, "else"))
				{
					var elseCommand = CompileSuite();
					return new IfCommand(condition, thencommand, elseCommand);
				}
				if (TryCompile(TokenType.Name, "elseif"))
				{
					var elseCommand = CompileIfCommand();
					return new IfCommand(condition, thencommand, elseCommand);
				}
			}
			LexicalAnalyzer.PushIndent(indent);
			return new IfCommand(condition, thencommand);
		}

		private ICommand CompileWhileCommand()
		{
			var condition = CompileExpression();
			var command = CompileSuite();
			return new WhileCommand(condition, command);
		}
		
		private ICommand CompileMethodCommand()
		{
			var token = CompileName(true);
			var name = token.Value;
			CompileToken(TokenType.Separator, "(");
			var parameters = CompileParameterExpressionList();
			CompileToken(TokenType.Separator, ")");
			Token returnType = null;
			if (TryCompile(TokenType.Name, "returns"))
				returnType = LexicalAnalyzer.NextToken();
			var body = CompileSuite();
			return new MethodCommand(name, parameters, body);
		}

		private ICommand CompileLetCommand()
		{
			var expressionCommand = CompileExpressionCommand();
			if (!TryCompile(TokenType.Operator, "="))
			{
				CompileEndOfCommand();
				return expressionCommand;
			}
			var valueExpression = CompileExpression();
			return expressionCommand.Expression switch
			{
				NameExpression => CompileNameExpression(expressionCommand, valueExpression),
				AttributeExpression => CompileAttributeExpression(expressionCommand, valueExpression),
				_ => throw new SyntaxErrorException("invalid assignment")
			};
		}

		private ICommand CompileReturnCommand()
		{
			if (TryPeekCompileEndOfCommand())
			{
				CompileEndOfCommand();
				return new ReturnCommand(null);
			}
			var command = new ReturnCommand(CompileExpression());
			CompileEndOfCommand();
			return command;
		}

		private ICommand CompileClassCommand()
		{
			var name = CompileName(true).Value;
			//IList<IExpression> inheritances = null;
			var body = CompileSuite(false);
			return new ClassCommand(name, null /* inheritances */, body);
		}

		private void SkipEmptyLines()
		{
			while (true)
			{
				var newIndentation = LexicalAnalyzer.NextIndent();
				var token = LexicalAnalyzer.NextToken();
				if (token == null)
					return;
				if (token.TokenType == TokenType.EndOfLine)
					continue;
				LexicalAnalyzer.PushToken(token);
				LexicalAnalyzer.PushIndent(newIndentation);
				return;
			}
		}

		private IExpression CompileBinaryLevel3Expression()
		{
			var expression = CompileTerm();
			if (expression == null)
				return null;
			var token = LexicalAnalyzer.NextToken();
			while (IsLevel3Operator(token))
			{
				var expression2 = CompileTerm();
				expression = new BinaryOperatorExpression(expression, expression2,
					CompileOperator(token.Value));
				token = LexicalAnalyzer.NextToken();
			}
			if (token != null)
				LexicalAnalyzer.PushToken(token);
			return expression;
		}

		private IExpression CompileBinaryLevel2Expression()
		{
			var expression = CompileBinaryLevel3Expression();
			if (expression == null)
				return null;
			var token = LexicalAnalyzer.NextToken();
			while (IsLevel2Operator(token))
			{
				var expression2 = CompileBinaryLevel3Expression();
				expression = new BinaryOperatorExpression(expression, expression2,
					CompileOperator(token.Value));
				token = LexicalAnalyzer.NextToken();
			}
			if (token != null)
				LexicalAnalyzer.PushToken(token);
			return expression;
		}

		private IExpression CompileOrExpression()
		{
			var expression = CompileAndExpression();
			if (expression == null)
				return null;
			while (TryCompile(TokenType.Name, "or"))
				expression = new BooleanExpression(expression, CompileAndExpression(), BooleanOperator.Or);
			return expression;
		}

		private IExpression CompileAndExpression()
		{
			var expression = CompileNotExpression();
			if (expression == null)
				return null;
			while (TryCompile(TokenType.Name, "and"))
				expression = new BooleanExpression(expression, CompileNotExpression(), BooleanOperator.And);
			return expression;
		}

		private IExpression CompileNotExpression() =>
			TryCompile(TokenType.Name, "not")
				? new NotExpression(CompileNotExpression())
				: CompileBinaryLevel0Expression();

		private IExpression CompileBinaryLevel0Expression()
		{
			var expression = CompileBinaryLevel1Expression();
			if (expression == null)
				return null;
			var token = LexicalAnalyzer.NextToken();
			while (IsLevel0Operator(token))
			{
				var expression2 = CompileBinaryLevel1Expression();
				expression = new CompareExpression(CompileCompareOperator(token.Value),
					expression, expression2);
				token = LexicalAnalyzer.NextToken();
			}
			if (token != null)
				LexicalAnalyzer.PushToken(token);
			return expression;
		}

		private IExpression CompileBinaryLevel1Expression()
		{
			var expression = CompileBinaryLevel2Expression();
			if (expression == null)
				return null;
			var token = LexicalAnalyzer.NextToken();
			while (IsLevel1Operator(token))
			{
				var expression2 = CompileBinaryLevel2Expression();
				expression = new BinaryOperatorExpression(expression, expression2,
					CompileOperator(token.Value));
				token = LexicalAnalyzer.NextToken();
			}
			if (token != null)
				LexicalAnalyzer.PushToken(token);
			return expression;
		}

		private IList<IExpression> CompileArgumentExpressionList()
		{
			var expressions = new List<IExpression>();
			var expression = CompileArgumentExpression();
			if (expression == null)
				return null;
			expressions.Add(expression);
			while (TryCompile(TokenType.Separator, ","))
				expressions.Add(CompileArgumentExpression());
			return expressions;
		}

		private IExpression CompileArgumentExpression()
		{
			var token = LexicalAnalyzer.NextToken();
			if (token == null)
				return null;
			if (token.TokenType == TokenType.Name)
			{
				var token2 = LexicalAnalyzer.NextToken();
				if (token2 != null)
					if (token2.TokenType == TokenType.Operator && token2.Value == "=")
						return new NamedArgumentExpression(token.Value, CompileExpression());
					else
						LexicalAnalyzer.PushToken(token2);
			}
			LexicalAnalyzer.PushToken(token);
			return CompileExpression();
		}

		private IExpression CompileTerm()
		{
			var term = CompileSimpleTerm();
			if (term == null)
				return null;
			while (true)
				if (TryCompile(TokenType.Operator, "."))
					term = new AttributeExpression(term, CompileName(true).Value);
				else if (TryCompile(TokenType.Separator, "("))
					term = CompileCallExpression(term);
				else if (TryCompile(TokenType.Separator, "["))
					term = CompileIndexedExpression(term);
				else
					break;
			return term;
		}

		private IExpression CompileCallExpression(IExpression term)
		{
			var expressions = CompileArgumentExpressionList();
			CompileToken(TokenType.Separator, ")");
			return new CallExpression(term, expressions);
		}

		private IExpression CompileIndexedExpression(IExpression term)
		{
			IExpression indexExpression = null;
			IExpression endExpression;
			if (!TryCompile(TokenType.Separator, ":"))
			{
				indexExpression = CompileExpression();
				if (!TryCompile(TokenType.Separator, ":"))
				{
					CompileToken(TokenType.Separator, "]");
					return indexExpression != null
						? new IndexedExpression(term, indexExpression)
						: new SlicedExpression(term, new SliceExpression(null, null));
				}
				if (TryCompile(TokenType.Separator, "]"))
					return new SlicedExpression(term,
						new SliceExpression(indexExpression, null /* endexpr */));
				endExpression = CompileExpression();
			}
			else
			{
				endExpression = CompileExpression();
			}
			CompileToken(TokenType.Separator, "]");
			return new SlicedExpression(term, new SliceExpression(indexExpression, endExpression));
		}

		private IExpression CompileSimpleTerm()
		{
			var token = LexicalAnalyzer.NextToken();
			if (token == null)
				return null;
			switch (token.TokenType)
			{
			case TokenType.String:
				return new ConstantExpression(token.Value);
			case TokenType.Integer:
				return new ConstantExpression(Convert.ToInt32(token.Value));
			case TokenType.Real:
				return new ConstantExpression(Convert.ToDouble(token.Value, CultureInfo.InvariantCulture));
			case TokenType.Boolean:
				return new ConstantExpression(Convert.ToBoolean(token.Value));
			case TokenType.Name:
				return MakeName(token.Value);
			case TokenType.Operator:
				if (token.Value == "-")
					return new NegateExpression(CompileTerm());
				else
					break;
			case TokenType.Separator:
				if (token.Value == "(")
				{
					var list = CompileExpressionList();
					CompileToken(TokenType.Separator, ")");
					return list == null
						? new ListExpression(new List<IExpression>(), true)
						: list.Count == 1
							? list[0]
							: new ListExpression(list, true);
				}
				if (token.Value == "[")
				{
					var expression = CompileList();
					CompileToken(TokenType.Separator, "]");
					return expression;
				}
				break;
			}
			LexicalAnalyzer.PushToken(token);
			return null;
		}

		private void CompileToken(TokenType type, string value)
		{
			var token = LexicalAnalyzer.NextToken();
			if (token == null || token.TokenType != type || token.Value != value)
				throw new ExpectedTokenException(value);
		}

		private void CompileToken(TokenType type)
		{
			var token = LexicalAnalyzer.NextToken();
			if (token == null || token.TokenType != type)
				throw new ExpectedTokenException(type.ToString());
		}

		private IExpression MakeName(string name) =>
			name switch
			{
				"None" => new ConstantExpression(null),
				"True" => new ConstantExpression(true),
				"False" => new ConstantExpression(false),
				_ => new NameExpression(name)
			};

		private Token CompileName(bool required)
		{
			var token = LexicalAnalyzer.NextToken();
			if (token == null && !required)
				return null;
			if (token == null ||
				token.TokenType != TokenType.Name && token.TokenType != TokenType.Operator)
				throw new NameExpectedException();
			return token;
		}

		private Token CompileName(string expected)
		{
			var token = LexicalAnalyzer.NextToken();
			if (token == null || token.TokenType != TokenType.Name || token.Value != expected)
				throw new ExpectedTokenException(expected);
			return token;
		}

		private bool TryPeekCompile(TokenType type, string value)
		{
			var token = LexicalAnalyzer.NextToken();
			if (token == null)
				return false;
			LexicalAnalyzer.PushToken(token);
			if (token.TokenType == type && token.Value == value)
				return true;
			return false;
		}

		private bool TryCompile(TokenType type, string value)
		{
			var token = LexicalAnalyzer.NextToken();
			if (token == null)
				return false;
			if (token.TokenType == type && token.Value == value)
				return true;
			LexicalAnalyzer.PushToken(token);
			return false;
		}

		private Token TryCompile(TokenType type)
		{
			var token = LexicalAnalyzer.NextToken();
			if (token == null)
				return null;
			if (token.TokenType == type)
				return token;
			LexicalAnalyzer.PushToken(token);
			return null;
		}
	}
}