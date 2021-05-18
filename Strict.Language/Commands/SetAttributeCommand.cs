﻿using Strict.Context;
using Strict.Language.Expressions;

namespace Strict.Language.Commands
{
	public class SetAttributeCommand : ICommand
	{
		public string Name { get; }
		public IExpression Expression { get; }
		public IExpression TargetExpression { get; }

		public SetAttributeCommand(IExpression targetExpression, string name, IExpression expression)
		{
			TargetExpression = targetExpression;
			Name = name;
			Expression = expression;
		}

		public void Accept(IVisitor visitor, IContext context) => visitor.Visit(this, visitor, context);
	}
}