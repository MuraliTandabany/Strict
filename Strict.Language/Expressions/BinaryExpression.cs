﻿using System;

namespace Strict.Language.Expressions
{
	public class BinaryExpression : IExpression
	{
		public IExpression Left { get; }
		public IExpression Right { get; }

		protected BinaryExpression(IExpression left, IExpression right)
		{
			Left = left ?? throw new ArgumentNullException(nameof(left));
			Right = right ?? throw new ArgumentNullException(nameof(right));
		}

		public object Visitor(IContext context) => null;
	}
}