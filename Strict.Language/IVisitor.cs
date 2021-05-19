using Strict.Context;
using Strict.Language.Commands;
using Strict.Language.Expressions;

namespace Strict.Language
{
	public interface IVisitor
	{
		void Visit(ClassCommand classCommand, IVisitor visitor, IContext context);
		void Visit(CompositeCommand compositeCommand, IVisitor visitor, IContext context);
		void Visit(ExpressionCommand expressionCommand, IVisitor visitor, IContext context);
		void Visit(ForCommand forCommand, IVisitor visitor, IContext context);
		void Visit(HasCommand hasCommand, IVisitor visitor, IContext context);
		void Visit(IfCommand ifCommand, IVisitor visitor, IContext context);
		void Visit(MethodCommand methodCommand, IVisitor visitor, IContext context);
		void Visit(ReturnCommand returnCommand, IVisitor visitor, IContext context);
		void Visit(SetAttributeCommand setAttributeCommand, IVisitor visitor, IContext context);
		void Visit(LetCommand letCommand, IVisitor visitor, IContext context);
		void Visit(WhileCommand whileCommand, IVisitor visitor, IContext context);

		object Visit(AttributeExpression attributeExpression, IVisitor visitor, IContext context);
		object Visit(CallExpression callExpression, IVisitor visitor, IContext context);
		object Visit(NameExpression nameExpression, IVisitor visitor, IContext context);

		object Visit(BinaryOperatorExpression binaryOperatorExpression, IVisitor visitor,
			IContext context);

		object Visit(BooleanExpression booleanExpression, IVisitor visitor, IContext context);
		object Visit(CompareExpression compareExpression, IVisitor visitor, IContext context);
	}
}