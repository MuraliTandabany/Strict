namespace Strict.Language.Expressions
{
	public class ParameterExpression : IExpression
	{
		public string Name { get; }
		public IExpression DefaultExpression { get; }
		public bool IsList { get; }

		public ParameterExpression(string name, IExpression defaultExpression, bool isList)
		{
			Name = name;
			DefaultExpression = defaultExpression;
			IsList = isList;
		}

		public object Visitor(IContext context) => null;
	}
}