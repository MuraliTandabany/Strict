using NUnit.Framework;
using Strict.Language;
using Strict.Language.Commands;
using Strict.Language.Expressions;

namespace Strict.Compiler.Tests
{
    internal class ParserTest
    {
        [TestCase("1+1", 1, 1, BinaryOperator.Add)]
        [TestCase("2-7", 2, 7, BinaryOperator.Subtract)]
        [TestCase("3/6", 3, 6, BinaryOperator.Divide)]
        [TestCase("4*5", 4, 5, BinaryOperator.Multiply)]
        public void BinaryExpressionOperatorTest(string expression, int left, int right,
            BinaryOperator binaryOperator)
        {
            var parser = new Parser(expression);
            var command = parser.CompileCommand();
            Assert.That(command, Is.TypeOf<ExpressionCommand>());
            var expressionCommand = (ExpressionCommand)command;
            Assert.That(expressionCommand.Expression, Is.TypeOf<BinaryOperatorExpression>());
            var binaryExpressionCommand =
                (BinaryOperatorExpression)expressionCommand.Expression;
            Assert.That(((ConstantExpression)binaryExpressionCommand.Left).Value,
                Is.EqualTo(left));
            Assert.That(((ConstantExpression)binaryExpressionCommand.Right).Value,
                Is.EqualTo(right));
            Assert.That(binaryExpressionCommand.Operator, Is.EqualTo(binaryOperator));
        }

        [TestCase("for i to 20\r\n")]
        [TestCase("for i to 20\r\n\tfor j to 20\r\n")]
        [TestCase("for i to functionThatReturnSomeRange(200)\r\n\tfor j to 20\r\n")]
        public void ForLoopParsingTest(string expression)
        {
            var parser = new Parser(expression);
            var commandList = parser.CompileCommandList();
            Assert.That(commandList, Is.TypeOf<ForCommand>());
            var forCommandList = (ForCommand)commandList;
            if (forCommandList.Command is ExpressionCommand) { }
            else if (forCommandList.Command is ForCommand) { }
            else if (forCommandList.Expression is CallExpression) { }
        }

        [TestCase("10 <  10")]
        [TestCase("20 <= 20")]
        [TestCase("30 >  30")]
        [TestCase("10 >= 30")]
        [TestCase("10 == 30")]
        [TestCase("10 <> 30")]
        [TestCase("10 != 30")]
        public void ComparisonOperatorParsingTest(string expression)
        {
            var parser = new Parser(expression);
            var commandList = parser.CompileCommandList();
        }

        [TestCase("5 + 5")]
        [TestCase("5 - 5")]
        [TestCase("5 * 5")]
        [TestCase("5 / 5")]
        [TestCase("5 ** 5")]
        public void BinaryOperatorParsingTest(string expression)
        {
            var parser = new Parser(expression);
            var commandList = parser.CompileCommandList();
        }

        [TestCase("let a = 10")]
        [TestCase(
            "let a = someFunction(parm1, parm2, parm3, parm4, parm5, parm6, parm7, parm8)")]
        [TestCase(
            "let a = someCrazyFunction(b(c(d(e(f(g(h(i(j(k(l(m(n(o(p(q(r(s(t(u(v(w(y(z(x)))))))))))))))))))))))))")]
        [TestCase("let a = 10\r\nlet b = \"abcdefghijklmnopqrstuvwxyz\"")]
        public void LetAssignmentTest(string expression)
        {
            var parser = new Parser(expression);
            var command = parser.CompileCommand();
        }

        [TestCase("print(a)")]
        [TestCase("a = print(b)")]
        [TestCase("a.b = c.d")]
        public void GeneralTest(string expression)
        {
            var parser = new Parser(expression);
            var command = parser.CompileCommand();
        }
    }
}