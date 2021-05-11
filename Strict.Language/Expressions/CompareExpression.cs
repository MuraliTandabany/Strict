namespace Strict.Language.Expressions
{
    public class CompareExpression : BinaryExpression
    {
        private ComparisonOperator Operation { get; }

        public CompareExpression(ComparisonOperator operation, IExpression left,
            IExpression right) : base(left, right) =>
            Operation = operation;
    }
}