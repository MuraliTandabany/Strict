﻿namespace Strict.Language.Expressions
{
	public class NameExpression : IExpression
	{
		public string Name { get; }
		public NameExpression(string name) => Name = name;
		public object Visitor(IContext context) => null;
	}
}