using System.Linq;
using NUnit.Framework;

namespace Strict.Language.Expressions.Tests;

public sealed class IfTests : TestExpressions
{
	[Test]
	public void ParseIncompleteIf() =>
		Assert.That(() => ParseExpression("if"),
			Throws.InstanceOf<If.MissingCondition>().With.Message.
				Contains(@"TestPackage\dummy.strict:line 2"));

	[Test]
	public void ParseInvalidSpaceAfterElseIsNotAllowed() =>
		Assert.That(() => ParseExpression("else "), Throws.InstanceOf<Type.ExtraWhitespacesFoundAtEndOfLine>());

	[Test]
	public void ParseJustElseIsNotAllowed() =>
		Assert.That(() => ParseExpression("else"),
			Throws.InstanceOf<If.UnexpectedElse>().With.Message.
				Contains(@"at TestPackage.dummy.Run in "));

	[Test]
	public void ParseIncompleteThen() =>
		Assert.That(() => ParseExpression("if bla is 5"), Throws.InstanceOf<If.MissingThen>());

	[Test]
	public void ParseWrongIndentation() =>
		Assert.That(() => ParseExpression("if bla is 5", "Run"),
			Throws.InstanceOf<Method.InvalidIndentation>());

	[Test]
	public void ParseIf() =>
		Assert.That(ParseExpression("if bla is 5", "\tlog.Write(\"Hey\")"),
			Is.EqualTo(new If(GetCondition(), GetThen(), null)));

	[Test]
	public void ParseMissingElseExpression() =>
		Assert.That(() => ParseExpression("if bla is 5", "\tRun", "else"),
			Throws.InstanceOf<If.UnexpectedElse>().With.Message.
				Contains(@"TestPackage\dummy.strict:line 4"));

	[Test]
	public void ParseIfElse() =>
		Assert.That(ParseExpression("if bla is 5", "\tlog.Write(\"Hey\")", "else", "\tRun"),
			Is.EqualTo(new If(GetCondition(), GetThen(), new MethodCall(null, method))).And.Not.
				EqualTo(new If(GetCondition(), GetThen(), null)));

	private MethodCall GetThen() =>
		new(new MemberCall(member), member.Type.Methods[0], new Text(type, "Hey"));

	private Binary GetCondition() =>
		new(new MemberCall(bla), binaryOperators.First(m => m.Name == BinaryOperator.Is), number);

	[Test]
	public void ReturnGetHashCode()
	{
		var ifExpression = (If)ParseExpression("if bla is 5", "\tRun");
		Assert.That(ifExpression.GetHashCode(),
			Is.EqualTo(ifExpression.Condition.GetHashCode() ^ ifExpression.Then.GetHashCode()));
	}
}