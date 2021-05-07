using System.IO;
using System.Collections.Generic;

namespace Strict.Compiler
{
    public class Parser
    {
        public static HashSet<string> Opslevel0 { get; } = new() { ">", "<", ">=", "<=", "<>", "==", "!=" };
        public static HashSet<string> Opslevel1 { get; } = new() { "+", "-" };
        public static HashSet<string> Opslevel2 { get; } = new() { "*", "/" };
        public static HashSet<string> Opslevel3 { get; } = new() { "**" };
        public static Token EndOfLineToken { get; } = new Token() { TokenType = TokenType.EndOfLine, Value = "\r\n" };
        
        private int IndentLevel { get; set; }
        public Lexer LexicalAnalyzer { get; }

        public Parser(Lexer lexer) => LexicalAnalyzer = lexer ?? throw new System.ArgumentNullException(nameof(lexer));

        public Parser(string text)
            : this(new Lexer(text))
        {
        }

        public Parser(TextReader reader)
            : this(new Lexer(reader))
        {
        }
    }
}
