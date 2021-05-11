using System.Collections.Generic;
using Strict.Exceptions;

namespace Strict.Language.Expressions
{
    public class CallExpression : IExpression
    {
        public IExpression TargetExpression { get; }

        public IList<IExpression> ArgumentExpressions { get; }
        private bool IsObject { get; }
        private bool HasNames { get; }

        public CallExpression(IExpression targetExpression,
            IList<IExpression> argumentExpressions)
        {
            TargetExpression = targetExpression;
            ArgumentExpressions = argumentExpressions;
            IsObject = TargetExpression is AttributeExpression;
            if (argumentExpressions == null)
                return;
            IList<string> names = new List<string>();
            foreach (var argumentExpression in argumentExpressions)
                if (argumentExpression is NamedArgumentExpression namedArgumentExpression)
                {
                    if (names.Contains(namedArgumentExpression.Name))
                        throw new SyntaxErrorException("keyword argument repeated");
                    names.Add(namedArgumentExpression.Name);
                    HasNames = true;
                }
                else if (HasNames)
                {
                    throw new SyntaxErrorException("non-keyword arg after keyword arg");
                }
        }
    }
}