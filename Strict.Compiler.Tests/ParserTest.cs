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
			var binaryExpressionCommand = (BinaryOperatorExpression)expressionCommand.Expression;
			Assert.That(((ConstantExpression)binaryExpressionCommand.Left).Value, Is.EqualTo(left));
			Assert.That(((ConstantExpression)binaryExpressionCommand.Right).Value, Is.EqualTo(right));
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
		[TestCase("let a = someFunction(parm1, parm2, parm3, parm4, parm5, parm6, parm7, parm8)")]
		[TestCase(
			"let a = someCrazyFunction(b(c(d(e(f(g(h(i(j(k(l(m(n(o(p(q(r(s(t(u(v(w(y(z(x)))))))))))))))))))))))))")]
		[TestCase("let a = 10\r\nlet b = \"abcdefghijklmnopqrstuvwxyz\"")]
		[TestCase("let a = (10+(20*(30-10)))")]
		[TestCase("let a = -10")]
		[TestCase("let a")]
		public void LetAssignmentTest(string expression)
		{
			var parser = new Parser(expression);
			var command = parser.CompileCommand();
		}

		[TestCase("let a = true")]
		[TestCase("let b = false")]
		public void BooleanTest(string expression)
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

		[TestCase("let a = [a, b, c]")]
		[TestCase("let b = a[20]")]
		[TestCase("let c = a[10][15]")]
		[TestCase("let d = a[10][15][20][25][30]")]
		[TestCase("let c = a[10:]")]
		[TestCase("let c = a[:10]")]
		[TestCase("let c = months_array[2:5]")]
		[TestCase("let c = [ [\"a\", 15.25], [\"b\", 15005.25]]")]
		public void IndexedExpressionTest(string expression)
		{
			var parser = new Parser(expression);
			var command = parser.CompileCommand();
		}

		[TestCase("implement c1\r\nmethod m1()\r\n\tprint()", "c1", new[] { "m1" })]
		[TestCase("implement c1\r\nmethod m1(p1, p2, *p3)\r\n\tprint(p1, p2, p3)", "c1",
			new[] { "m1" })]
		[TestCase("implement c1\r\nmethod m1(t1, t2, val = 5)\r\n\tprint()", "c1", new[] { "m1" })]
		public void ClassExpressionTest(string expression, string className, string[] methodNames)
		{
			var parser = new Parser(expression);
			var command = parser.CompileCommand();
			Assert.That(command, Is.TypeOf<ClassCommand>());
			var classCommand = (ClassCommand)command;
			Assert.That(classCommand.Name, Is.EqualTo(className));
		}

		[TestCase(
			"implement c1\r\nmethod m1(*param)\r\n\tprint()\r\nlet array = [10,20,30]\r\nc1.m1(array)")]
		public void ListParameterExpressionTest(string expression)
		{
			var parser = new Parser(expression);
			var command = parser.CompileCommand();
		}

		[TestCase("while a == true\r\n\tprint(a)")]
		[TestCase("while (a == true) and (b == false)\r\n\tprint(a, b)")]
		[TestCase("while (a == true) or (b == false)\r\n\tprint(a, b)")]
		[TestCase(
			"while ((a == true) and (b == false)) or (not (a == true) or not (b == false))\r\n\tprint(a, b)")]
		[TestCase("while not ((a == true) or (b == false)) \r\n\tprint(a, b)")]
		public void WhileExpressionTest(string expression)
		{
			var parser = new Parser(expression);
			var command = parser.CompileCommand();
		}

		[TestCase("if a == 10\r\n\tprint(a, b)")]
		[TestCase("if a >= 10\r\n\tprint(a, b)")]
		[TestCase("if a != 10\r\n\tprint(a, b)")]
		[TestCase("if a <> 10\r\n\tprint(a, b)")]
		[TestCase("if a > 10 and 10 <= a\r\n\tprint(a, b)")]
		[TestCase("if a >= 10 or 10 <= a\r\n\tprint(a, b)")]
		[TestCase("if a <= 10 and ((10 <= a) and not (a >= 10))\r\n\tprint(a, b)")]
		[TestCase("if a < 10 and ((10 <= a) and not (a >= 10))\r\n\tprint(a, b)")]
		[TestCase("if a <> 10\r\n\tprint(a, b)\r\nelse\r\n\tprint(a, b)")]
		[TestCase("if a <> 10\r\n\tprint(a, b)\r\nelseif\r\n\tprint(a, b)\r\nelse\r\n\tprint(a, b)")]
		public void IfElseElseIfExpressionTest(string expression)
		{
			var parser = new Parser(expression);
			var command = parser.CompileCommand();
		}

		[TestCase("has log")]
		[TestCase("has system.log")]
		[TestCase("has log, user, system.kernel, system.user32")]
		public void HasExpressionTest(string expression)
		{
			var parser = new Parser(expression);
			var command = parser.CompileCommand();
		}

		[TestCase("method p1(p = true)\r\n\treturn p ")]
		public void ReturnCommandTest(string expression)
		{
			var parser = new Parser(expression);
			var command = parser.CompileCommand();
		}

		[TestCase("method x(y)\n someCode()")]
		public void TestEspacesToOpenBlockOfCodeSource(string expression)
		{
			var parser = new Parser(expression);
			var command = parser.CompileCommand();
		}

		[TestCase("method x(y) returns Number\n someCode()")]
		public void SetReturnTypeFromMethodTest(string expression)
		{
			var parser = new Parser(expression);
			var command = parser.CompileCommand();
		}
	}
}