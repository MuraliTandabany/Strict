using NUnit.Framework;

namespace Strict.Language.Tests;

public class TypeTests
{
	[SetUp]
	public void CreatePackage()
	{
		package = new TestPackage();
		new Type(package, Base.App, null!).Parse("Run");
	}

	private Package package = null!;

	[Test]
	public void AddingTheSameNameIsNotAllowed() =>
		Assert.Throws<Type.TypeAlreadyExistsInPackage>(() => new Type(package, "App", null!));

	[Test]
	public void EmptyLineIsNotAllowed() =>
		Assert.That(() => new Type(package, Base.Count, null!).Parse("\n"),
			Throws.InstanceOf<Type.EmptyLineIsNotAllowed>().With.Message.Contains("line 1"));

	[Test]
	public void WhitespacesAreNotAllowed()
	{
		Assert.That(() => new Type(package, Base.Count, null!).Parse(" "),
			Throws.InstanceOf<Type.ExtraWhitespacesFoundAtBeginningOfLine>());
		Assert.That(() => new Type(package, "Program", null!).Parse(@"Run
 "), Throws.InstanceOf<Type.ExtraWhitespacesFoundAtBeginningOfLine>());
		Assert.That(() => new Type(package, Base.HashCode, null!).Parse("has\t"),
			Throws.InstanceOf<Type.ExtraWhitespacesFoundAtEndOfLine>());
	}

	[Test]
	public void TypeParsersMustStartWithImplementOrHas() =>
		Assert.That(() => new Type(package, Base.Count, null!).Parse(@"Run
	log.WriteLine"),
			Throws.InstanceOf<Type.TypeHasNoMembersAndThusMustBeATraitWithoutMethodBodies>());

	[Test]
	public void JustMembersIsNotValidCode() =>
		Assert.That(
			() => new Type(package, Base.Count, null!).Parse(new[] { "has log", "has count" }),
			Throws.InstanceOf<Type.NoMethodsFound>());

	[Test]
	public void GetUnknownTypeWillCrash() =>
		Assert.Throws<Context.TypeNotFound>(() => package.GetType(Base.Computation));

	[Test]
	public void TypeNameMustBeWord() =>
		Assert.Throws<Context.NameMustBeAWordWithoutAnySpecialCharactersOrNumbers>(() =>
			new Member(package.GetType(Base.App), "blub7", null!));

	[Test]
	public void ImportMustBeFirst() =>
		Assert.That(() => new Type(package, "Program", null!).Parse(@"has number
import TestPackage"), Throws.InstanceOf<ParsingFailed>().With.InnerException.
			InstanceOf<Type.ImportMustBeFirst>());

	[Test]
	public void ImportMustBeValidPackageName() =>
		Assert.That(() => new Type(package, "Program", null!).Parse(@"import $YI(*SI"),
			Throws.InstanceOf<ParsingFailed>().With.InnerException.
				InstanceOf<Type.PackageNotFound>());

	[Test]
	public void Import()
	{
		var program = new Type(package, "Program", null!).Parse(@"import TestPackage
has number
GetNumber returns Number
	return number");
		Assert.That(program.Imports[0].Name, Is.EqualTo(nameof(TestPackage)));
	}

	[Test]
	public void ImplementAnyIsImplicitAndNotAllowed() =>
		Assert.That(() => new Type(package, "Program", null!).Parse(@"implement Any"),
			Throws.InstanceOf<ParsingFailed>().With.InnerException.
				InstanceOf<Type.ImplementAnyIsImplicitAndNotAllowed>());

	[Test]
	public void ImplementMustBeBeforeMembersAndMethods() =>
		Assert.That(() => new Type(package, "Program", null!).Parse(@"has log
implement App"),
			Throws.InstanceOf<ParsingFailed>().With.InnerException.
				InstanceOf<Type.ImplementMustComeBeforeMembersAndMethods>());

	[Test]
	public void MembersMustComeBeforeMethods() =>
		Assert.That(() => new Type(package, "Program", null!).Parse(@"Run
has log"),
			Throws.InstanceOf<ParsingFailed>().With.InnerException.
				InstanceOf<Type.MembersMustComeBeforeMethods>());

	[Test]
	public void SimpleApp() =>
		CheckApp(new Type(package, "Program", null!).Parse(@"implement App
has log
Run
	log.Write(""Hello World!"")"));

	private static void CheckApp(Type program)
	{
		Assert.That(program.Implements[0].Name, Is.EqualTo(Base.App));
		Assert.That(program.Members[0].Name, Is.EqualTo("log"));
		Assert.That(program.Methods[0].Name, Is.EqualTo("Run"));
		Assert.That(program.IsTrait, Is.False);
	}

	[Test]
	public void AnotherApp() =>
		CheckApp(new Type(package, "Program", null!).Parse(@"implement App
has log
Run
	for number in Range(0, 10)
		log.Write(""Counting: "" + number)"));

	[Test]
	public void MustImplementAllTraitMethods() =>
		Assert.That(() => new Type(package, "Program", null!).Parse(@"implement App
add(number)
	return one + 1"),
			Throws.InstanceOf<Type.MustImplementAllTraitMethods>());

	[Test]
	public void TraitMethodsMustBeImplemented() =>
		Assert.That(() => new Type(package, "Program", null!).Parse(@"implement App
Run"),
			Throws.InstanceOf<Type.MethodMustBeImplementedInNonTraitType>());

	[Test]
	public void Trait()
	{
		var app = new Type(package, "DummyApp", null!).Parse("Run");
		Assert.That(app.IsTrait, Is.True);
		Assert.That(app.Name, Is.EqualTo("DummyApp"));
		Assert.That(app.Methods[0].Name, Is.EqualTo("Run"));
	}

	[Test]
	public void FileExtensionMustBeStrict() =>
		Assert.ThrowsAsync<Type.FileExtensionMustBeStrict>(() =>
			new Type(package, "DummyApp", null!).ParseFile("test.txt"));

	[Test]
	public void FilePathMustMatchPackageName() =>
		Assert.ThrowsAsync<Type.FilePathMustMatchPackageName>(() =>
			new Type(package, "DummyApp", null!).ParseFile("test.strict"));

	[Test]
	public void FilePathMustMatchMainPackageName() =>
		Assert.ThrowsAsync<Type.FilePathMustMatchPackageName>(() =>
			new Type(new Package(package, nameof(TypeTests)), "DummyApp", null!).ParseFile(
				nameof(TypeTests) + "\\test.strict"));
}