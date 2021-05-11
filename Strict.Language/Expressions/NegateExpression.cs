﻿namespace Strict.Language.Expressions
{
    public class NegateExpression : IExpression
    {
        private IExpression Expression { get; }

        public NegateExpression(IExpression expression) => Expression = expression;
    }
}