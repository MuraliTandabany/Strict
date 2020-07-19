using System.Collections.Generic;
using NUnit.Framework;
using Strict.Tokens;

namespace Strict.Language.Tests
{
	public class ExpressionParserTests : ExpressionParser
	{
		[SetUp]
		public void CreateType() =>
			type = new Type(new TestPackage(), nameof(TypeTests), this).Parse(@"has log
Run
	log.WriteLine");

		private Type type;

		[Test]
		public void ParsingHappensAfterCallingBody()
		{
			Assert.That(Expressions, Is.Empty);
			Assert.That(type.Methods[0].Body, Is.Not.Null);
			Assert.That(Expressions, Is.Not.Empty);
			Assert.That(type.Methods[0].Body.Expressions, Is.Not.Empty);
			Assert.That(type.Methods[0].Body.Expressions[0].ReturnType,
				Is.EqualTo(type.Methods[0].ReturnType));
		}

		public override void ParseOldTODO(Method method, List<Token> tokens)
		{
			if (tokens.Count == 3)
				tokens.Clear();
			expressions.Add(new TestExpression(method.ReturnType));
		}

		public override Expression Parse(Method method, string lines) => null;

		public class TestExpression : Expression
		{
			public TestExpression(Type returnType) : base(returnType) { }
		}

		[Test]
		public void ThereMustBeNoTokensLeft() =>
			Assert.Throws<MethodBody.UnprocessedTokensAtEndOfFile>(() =>
				new MethodBody(type.Methods[0], this, new[] { "Dummy", "\tdummy" }));

		[Test]
		public void CompareExpressions()
		{
			var expression = new TestExpression(type);
			Assert.That(expression, Is.EqualTo(new TestExpression(type)));
			Assert.That(expression.GetHashCode(),
				Is.EqualTo(new TestExpression(type).GetHashCode()));
			Assert.That(new TestExpression(type.Methods[0].ReturnType), Is.Not.EqualTo(new TestExpression(type)));
			Assert.That(expression.Equals((object)new TestExpression(type)), Is.True);
		}
	}
}