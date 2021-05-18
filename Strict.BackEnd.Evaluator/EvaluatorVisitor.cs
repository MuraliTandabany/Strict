using Strict.Context;
using Strict.BackEnd.Evaluator.Commands;
using Strict.BackEnd.Evaluator.Expressions;
using Strict.Language;
using Strict.Language.Commands;
using Strict.Language.Expressions;

namespace Strict.BackEnd.Evaluator
{
	public class EvaluatorVisitor : IVisitor
	{
		public void Visit(ClassCommand classCommand, IVisitor visitor, IContext context) =>
			ClassCommandVisitor.VisitCommand(classCommand, visitor, context);

		public void Visit(CompositeCommand compositeCommand, IVisitor visitor, IContext context) =>
			CompositeCommandVisitor.VisitCommand(compositeCommand, visitor, context);

		public void Visit(ExpressionCommand expressionCommand, IVisitor visitor, IContext context) =>
			ExpressionCommandVisitor.VisitCommand(expressionCommand, visitor, context);

		public void Visit(ForCommand forCommand, IVisitor visitor, IContext context) =>
			ForCommandVisitor.VisitCommand(forCommand, visitor, context);

		public void Visit(HasCommand hasCommand, IVisitor visitor, IContext context) =>
			HasCommandVisitor.VisitCommand(hasCommand, visitor, context);

		public void Visit(IfCommand ifCommand, IVisitor visitor, IContext context) =>
			IfCommandVisitor.VisitCommand(ifCommand, visitor, context);

		public void Visit(MethodCommand methodCommand, IVisitor visitor, IContext context) =>
			MethodCommandVisitor.VisitCommand(methodCommand, visitor, context);

		public void Visit(ReturnCommand returnCommand, IVisitor visitor, IContext context) =>
			ReturnCommandVisitor.VisitCommand(returnCommand, visitor, context);

		public void
			Visit(SetAttributeCommand setAttributeCommand, IVisitor visitor, IContext context) =>
			SetAttributeCommandVisitor.VisitCommand(setAttributeCommand, visitor, context);

		public void Visit(SetCommand setCommand, IVisitor visitor, IContext context) =>
			SetCommandVisitor.VisitCommand(setCommand, visitor, context);

		public void Visit(WhileCommand whileCommand, IVisitor visitor, IContext context) =>
			WhileCommandVisitor.VisitCommand(whileCommand, visitor, context);

		public object
			Visit(AttributeExpression attributeExpression, IVisitor visitor, IContext context) =>
			AttributeExpressionVisitor.VisitExpression(attributeExpression, visitor, context);

		public object Visit(CallExpression callExpression, IVisitor visitor, IContext context) =>
			CallExpressionVisitor.VisitExpression(callExpression, visitor, context);

		public object Visit(NameExpression nameExpression, IVisitor visitor, IContext context) =>
			NameExpressionVisitor.VisitExpression(nameExpression, visitor, context);

		public object Visit(BinaryOperatorExpression binaryOperatorExpression, IVisitor visitor,
			IContext context) =>
			BinaryOperatorExpressionVisitor.VisitExpression(binaryOperatorExpression, visitor, context);

		public object Visit(BooleanExpression booleanExpression, IVisitor visitor, IContext context) =>
			BooleanExpressionVisitor.VisitExpression(booleanExpression, visitor, context);

		public object Visit(CompareExpression compareExpression, IVisitor visitor, IContext context) =>
			CompareExpressionVisitor.VisitExpression(compareExpression, visitor, context);
	}
}