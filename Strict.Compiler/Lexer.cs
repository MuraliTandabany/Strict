using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace Strict.Compiler
{
	public class Lexer : IDisposable
	{
		private const char StringChar = '"';
		private const char QuotedStringChar = '\'';
		private const char EscapeChar = '\\';
		private const char CommentChar = '#';
		private static readonly HashSet<char> Operators = new()
		{
			'+',
			'-',
			'/',
			'*',
			'=',
			'.',
			'>',
			'<',
			'|'
		};
		private static readonly HashSet<char> OperatorStarts = new() { '!' };
		private static readonly HashSet<char> Separators = new()
		{
			'(',
			')',
			'[',
			']',
			'{',
			'}',
			',',
			':',
			';'
		};
		private static readonly HashSet<string> OtherOperators = new()
		{
			"**",
			"<=",
			">=",
			"==",
			"<>",
			"!=",
			"|>",
			"<|",
			"++",
			"--"
		};

		private readonly Stack<int> lastChars = new();

		private readonly TextReader reader;
		private readonly Stack<Token> tokenStack = new();
		private int LastIndentation = -1;

		public Lexer(string text) =>
			reader = text == null
				? throw new ArgumentNullException(nameof(text))
				: new StringReader(text);

		public Lexer(TextReader reader) =>
			this.reader = reader ?? throw new ArgumentNullException(nameof(reader));

		public void Dispose()
		{
			Dispose(true);
			GC.SuppressFinalize(this);
		}

		public void PushIndent(int indent) => LastIndentation = indent;

		public int NextIndent()
		{
			var indent = 0;
			if (LastIndentation >= 0)
			{
				indent = LastIndentation;
				LastIndentation = -1;
				return indent;
			}
			int ich;
			for (ich = NextChar(); ich >= 0 && IsSpace((char)ich); ich = NextChar())
				indent++;
			PushChar(ich);
			return indent;
		}

		public Token NextToken()
		{
			if (tokenStack.Count != 0)
				return tokenStack.Pop();
			var ich = NextCharSkipBlanks();
			if (ich < 0)
				return null;
			var ch = (char)ich;
			if (ch == '\n' || ch == '\r')
				return NextEndOfLine(ch);
			if (char.IsDigit(ch))
				return NextInteger(ch);
			if (char.IsLetter(ch) || ch == '_')
				return NextName(ch);
			if (ch == StringChar)
				return NextString(StringChar);
			if (ch == QuotedStringChar)
				return NextString(QuotedStringChar);
			if (Separators.Contains(ch))
				return NextSeparator(ch);
			if (Operators.Contains(ch) || OperatorStarts.Contains(ch))
				return NextOperator(ch);
			throw new InvalidDataException("Unknown input");
		}

		public void Dispose(bool dispose)
		{
			if (dispose && reader != null)
				reader.Dispose();
		}

		internal void PushToken(Token token) => tokenStack.Push(token);

		private static bool IsSpace(char ch) => char.IsWhiteSpace(ch) && ch != '\r' && ch != '\n';

		private Token NextEndOfLine(char ch)
		{
			var value = ch.ToString();
			if (ch != '\r')
				return new Token { TokenType = TokenType.EndOfLine, Value = value };
			var ich2 = NextChar();
			if (ich2 < 0 || (char)ich2 != '\n')
				PushChar(ich2);
			else
				value += (char)ich2;
			return new Token { TokenType = TokenType.EndOfLine, Value = value };
		}

		private Token NextOperator(char ch)
		{
			var ich2 = NextChar();
			if (ich2 >= 0)
			{
				var ch2 = (char)ich2;
				var op = ch + ch2.ToString();
				if (OtherOperators.Contains(op))
					return new Token { TokenType = TokenType.Operator, Value = op };
				PushChar(ich2);
			}
			else
			{
				PushChar(ich2);
			}
			if (Operators.Contains(ch))
				return new Token { TokenType = TokenType.Operator, Value = ch.ToString() };
			throw new InvalidDataException("Unknown input");
		}

		private static Token NextSeparator(char ch) =>
			new() { TokenType = TokenType.Separator, Value = ch.ToString() };

		private Token NextString(char endChar)
		{
			var sb = new StringBuilder();
			var ich = NextChar();
			if (ich < 0)
				return new Token { Value = sb.ToString(), TokenType = TokenType.String };
			var ch = (char)ich;
			if (ch == endChar)
			{
				var ich2 = NextChar();
				if (ich2 >= 0 && (char)ich2 == endChar)
					return NextMultilineString(endChar);
				PushChar(ich2);
			}
			while (ich >= 0 && ch != endChar)
			{
				if (ch == EscapeChar)
					ch = (char)NextChar();
				sb.Append(ch);
				ich = NextChar();
				if (ich >= 0)
					ch = (char)ich;
			}
			return new Token { Value = sb.ToString(), TokenType = TokenType.String };
		}

		private Token NextMultilineString(char endChar)
		{
			var sb = new StringBuilder();
			while (true)
			{
				var ich = NextChar();
				if (ich < 0)
				{
					PushChar(ich);
					break;
				}
				var ch = (char)ich;
				if (ch == endChar)
				{
					var ich2 = NextChar();
					if (ich2 >= 0 && (char)ich2 == endChar)
					{
						var ich3 = NextChar();
						if (ich3 >= 0 && (char)ich3 == endChar)
							break;
						PushChar(ich3);
					}
					PushChar(ich2);
				}
				if (ch == EscapeChar)
					ch = (char)NextChar();
				sb.Append(ch);
			}
			return new Token { Value = sb.ToString(), TokenType = TokenType.String };
		}

		private Token NextInteger(char ch)
		{
			var integer = ch.ToString();
			var ich = NextChar();
			while (ich >= 0 && char.IsDigit((char)ich))
			{
				integer += (char)ich;
				ich = NextChar();
			}
			if (ich >= 0 && (char)ich == '.')
				return NextReal(integer);
			PushChar(ich);
			return new Token { Value = integer, TokenType = TokenType.Integer };
		}

		private Token NextReal(string integerPart)
		{
			var real = integerPart + ".";
			var ich = NextChar();
			while (ich >= 0 && char.IsDigit((char)ich))
			{
				real += (char)ich;
				ich = NextChar();
			}
			PushChar(ich);
			return new Token { Value = real, TokenType = TokenType.Real };
		}

		private Token NextName(char ch)
		{
			var name = ch.ToString();
			var ich = NextChar();
			while (ich >= 0 && (char.IsLetterOrDigit((char)ich) || (char)ich == '_'))
			{
				name += (char)ich;
				ich = NextChar();
			}
			PushChar(ich);
			var token = new Token { Value = name, TokenType = TokenType.Name };
			if (name == "true" || name == "false")
				token.TokenType = TokenType.Boolean;
			return token;
		}

		private int NextCharSkipBlanks()
		{
			var ich = NextChar();
			while (ich >= 0)
			{
				var ch = (char)ich;
				if (!char.IsWhiteSpace(ch) || ch == '\n' || ch == '\r')
					break;
				ich = NextChar();
			}
			return ich;
		}

		private void PushChar(int ch) => lastChars.Push(ch);

		private int NextChar()
		{
			var ich = NextSimpleChar();
			if (ich < 0 || (char)ich != CommentChar)
				return ich;
			while (ich >= 0 && (char)ich != '\r' && (char)ich != '\n')
				ich = NextSimpleChar();
			return ich;
		}

		private int NextSimpleChar() =>
			lastChars.Count > 0
				? lastChars.Pop()
				: reader.Read();
	}
}