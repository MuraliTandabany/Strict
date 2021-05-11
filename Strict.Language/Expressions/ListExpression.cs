using System.Collections.Generic;

namespace Strict.Language.Expressions
{
    public class ListExpression : IExpression
    {
        private IList<IExpression> Expressions { get; }
        public bool IsReadonly { get; }

        public ListExpression(IList<IExpression> expressions) : this(expressions, false) { }

        public ListExpression(IList<IExpression> expressions, bool isReadonly)
        {
            Expressions = expressions;
            IsReadonly = isReadonly;
        }
    }
}