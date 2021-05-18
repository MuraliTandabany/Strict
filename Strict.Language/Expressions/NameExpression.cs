using Strict.Context;

namespace Strict.Language.Expressions
{
	public class NameExpression : IExpression
	{
		public string Name { get; }
		public NameExpression(string name) => Name = name;

		public object Accept(IVisitor visitor, IContext context) =>
			visitor.Visit(this, visitor, context);
	}
}