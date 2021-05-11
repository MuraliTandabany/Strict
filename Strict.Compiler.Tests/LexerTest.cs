using System.Linq;
using NUnit.Framework;

namespace Strict.Compiler.Tests
{
    public class LexerTest
    {
        private static void AssertToken(Token tokenFromLexer, Token expectedToken)
        {
            Assert.That(tokenFromLexer.TokenType, Is.EqualTo(expectedToken.TokenType));
            Assert.That(tokenFromLexer.Value, Is.EqualTo(expectedToken.Value));
        }

        [TestCase("10.848", TokenType.Real)]
        [TestCase("0.0000000001", TokenType.Real)]
        [TestCase("1.001", TokenType.Real)]
        [TestCase("00000000000000000000000001.001", TokenType.Real)]
        public void TestTokenizerFloatNumber(string doubleValue, TokenType expectedToken)
        {
            var lexer = new Lexer(doubleValue);
            AssertToken(lexer.NextToken(),
                new Token { Value = doubleValue, TokenType = expectedToken });
        }

        [TestCase("=", TokenType.Operator)]
        [TestCase("+", TokenType.Operator)]
        [TestCase("-", TokenType.Operator)]
        [TestCase("*", TokenType.Operator)]
        [TestCase(">=", TokenType.Operator)]
        [TestCase("<=", TokenType.Operator)]
        [TestCase("!=", TokenType.Operator)]
        [TestCase("==", TokenType.Operator)]
        [TestCase("<>", TokenType.Operator)]
        public void TestTokenizerOperators(string @operator, TokenType expectedToken)
        {
            var lexer = new Lexer(@operator);
            AssertToken(lexer.NextToken(),
                new Token { Value = @operator, TokenType = expectedToken });
        }

        [TestCase("\"Hello\"", TokenType.String)]
        [TestCase("\"Hello - 1\r\nHello - 2\r\nHello - 3\"", TokenType.String)]
        public void TestTokenizerStrings(string str, TokenType expectedToken)
        {
            var lexer = new Lexer(str);
            AssertToken(lexer.NextToken(),
                new Token { Value = str.Replace("\"", ""), TokenType = expectedToken });
        }

        [Test]
        public void TestTokenizerPipe()
        {
            const string source = "let test = 10 |> print <| function <| print ";
            var lexer = new Lexer(source);
            AssertToken(lexer.NextToken(),
                new Token { Value = "let", TokenType = TokenType.Name });
            AssertToken(lexer.NextToken(),
                new Token { Value = "test", TokenType = TokenType.Name });
            AssertToken(lexer.NextToken(),
                new Token { Value = "=", TokenType = TokenType.Operator });
            AssertToken(lexer.NextToken(),
                new Token { Value = "10", TokenType = TokenType.Integer });
            AssertToken(lexer.NextToken(),
                new Token { Value = "|>", TokenType = TokenType.Operator });
            AssertToken(lexer.NextToken(),
                new Token { Value = "print", TokenType = TokenType.Name });
            AssertToken(lexer.NextToken(),
                new Token { Value = "<|", TokenType = TokenType.Operator });
            AssertToken(lexer.NextToken(),
                new Token { Value = "function", TokenType = TokenType.Name });
            AssertToken(lexer.NextToken(),
                new Token { Value = "<|", TokenType = TokenType.Operator });
        }

        [Test]
        public void TestEmptyLines()
        {
            const int sourceRowsCount = 3000;
            var source = string.Join("\r\n",
                Enumerable.Range(0, sourceRowsCount).Select(_ => ""));
            var lexer = new Lexer(source);
            while (true)
            {
                var token = lexer.NextToken();
                if (token == null)
                    break;
                AssertToken(token,
                    new Token { Value = "\r\n", TokenType = TokenType.EndOfLine });
            }
        }

        [Test]
        public void TestTokenizerChainLoop()
        {
            const string source = @"
for i = 0 to 10
    for j = 0 to 20 * i
        for k = 0 to 30 * j

print i j k";
            var lexer = new Lexer(source);
            var nextIndentation = lexer.NextIndent();
            Assert.That(nextIndentation, Is.EqualTo(0));
            AssertToken(lexer.NextToken(),
                new Token { Value = "\r\n", TokenType = TokenType.EndOfLine });
            nextIndentation = lexer.NextIndent();
            Assert.That(nextIndentation, Is.EqualTo(0));
            AssertToken(lexer.NextToken(),
                new Token { Value = "for", TokenType = TokenType.Name });
            AssertToken(lexer.NextToken(),
                new Token { Value = "i", TokenType = TokenType.Name });
            AssertToken(lexer.NextToken(),
                new Token { Value = "=", TokenType = TokenType.Operator });
            AssertToken(lexer.NextToken(),
                new Token { Value = "0", TokenType = TokenType.Integer });
            AssertToken(lexer.NextToken(),
                new Token { Value = "to", TokenType = TokenType.Name });
            AssertToken(lexer.NextToken(),
                new Token { Value = "10", TokenType = TokenType.Integer });
            AssertToken(lexer.NextToken(),
                new Token { Value = "\r\n", TokenType = TokenType.EndOfLine });
            nextIndentation = lexer.NextIndent();
            Assert.That(nextIndentation, Is.EqualTo(4));
            AssertToken(lexer.NextToken(),
                new Token { Value = "for", TokenType = TokenType.Name });
            AssertToken(lexer.NextToken(),
                new Token { Value = "j", TokenType = TokenType.Name });
            AssertToken(lexer.NextToken(),
                new Token { Value = "=", TokenType = TokenType.Operator });
            AssertToken(lexer.NextToken(),
                new Token { Value = "0", TokenType = TokenType.Integer });
            AssertToken(lexer.NextToken(),
                new Token { Value = "to", TokenType = TokenType.Name });
            AssertToken(lexer.NextToken(),
                new Token { Value = "20", TokenType = TokenType.Integer });
            AssertToken(lexer.NextToken(),
                new Token { Value = "*", TokenType = TokenType.Operator });
            AssertToken(lexer.NextToken(),
                new Token { Value = "i", TokenType = TokenType.Name });
            AssertToken(lexer.NextToken(),
                new Token { Value = "\r\n", TokenType = TokenType.EndOfLine });
            nextIndentation = lexer.NextIndent();
            Assert.That(nextIndentation, Is.EqualTo(8));
            AssertToken(lexer.NextToken(),
                new Token { Value = "for", TokenType = TokenType.Name });
            AssertToken(lexer.NextToken(),
                new Token { Value = "k", TokenType = TokenType.Name });
            AssertToken(lexer.NextToken(),
                new Token { Value = "=", TokenType = TokenType.Operator });
            AssertToken(lexer.NextToken(),
                new Token { Value = "0", TokenType = TokenType.Integer });
            AssertToken(lexer.NextToken(),
                new Token { Value = "to", TokenType = TokenType.Name });
            AssertToken(lexer.NextToken(),
                new Token { Value = "30", TokenType = TokenType.Integer });
            AssertToken(lexer.NextToken(),
                new Token { Value = "*", TokenType = TokenType.Operator });
            AssertToken(lexer.NextToken(),
                new Token { Value = "j", TokenType = TokenType.Name });
            AssertToken(lexer.NextToken(),
                new Token { Value = "\r\n", TokenType = TokenType.EndOfLine });
            nextIndentation = lexer.NextIndent();
            Assert.That(nextIndentation, Is.EqualTo(0));
            AssertToken(lexer.NextToken(),
                new Token { Value = "\r\n", TokenType = TokenType.EndOfLine });
            nextIndentation = lexer.NextIndent();
            Assert.That(nextIndentation, Is.EqualTo(0));
            AssertToken(lexer.NextToken(),
                new Token { Value = "print", TokenType = TokenType.Name });
            AssertToken(lexer.NextToken(),
                new Token { Value = "i", TokenType = TokenType.Name });
            AssertToken(lexer.NextToken(),
                new Token { Value = "j", TokenType = TokenType.Name });
            AssertToken(lexer.NextToken(),
                new Token { Value = "k", TokenType = TokenType.Name });
        }

        [Test]
        public void TestMultiLineString()
        {
            var source = @""""""" this is a very
        long string if I had the
        energy to type more and more ...""""""";
            var lexerExpectedValueResult =
                " this is a very\r\n        long string if I had the\r\n        energy to type more and more ...";
            var lexer = new Lexer(source);
            AssertToken(lexer.NextToken(),
                new Token { Value = lexerExpectedValueResult, TokenType = TokenType.String });
        }

        [Test]
        public void TestTokenizerTinySource()
        {
            var source = @"someNumber = 10
someNumber = someNumber + 10
print(someNumber)";
            var lexer = new Lexer(source);
            AssertToken(lexer.NextToken(),
                new Token { Value = "someNumber", TokenType = TokenType.Name });
            AssertToken(lexer.NextToken(),
                new Token { Value = "=", TokenType = TokenType.Operator });
            AssertToken(lexer.NextToken(),
                new Token { Value = "10", TokenType = TokenType.Integer });
        }
    }
}