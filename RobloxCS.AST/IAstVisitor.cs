using RobloxCS.AST.Expressions;
using RobloxCS.AST.Generics;
using RobloxCS.AST.Prefixes;
using RobloxCS.AST.Statements;
using RobloxCS.AST.Suffixes;
using RobloxCS.AST.Types;

namespace RobloxCS.AST;

public interface IAstVisitor {
    public void Visit(Block node);
    public void Visit(Expression node);
    public void Visit(FunctionArgs node);
    public void Visit(FunctionBody node);
    public void Visit(Parameter node);
    public void Visit(Prefix node);
    public void Visit(Statement node);
    public void Visit(Suffix node);
    public void Visit(Var node);

    public void Visit(AnonymousFunction node);
    public void Visit(BooleanExpression node);
    public void Visit(FunctionCall node);
    public void Visit(NumberExpression node);
    public void Visit(StringExpression node);
    public void Visit(SymbolExpression node);
    public void Visit(TableConstructor node);
    public void Visit(TableField node);
    public void Visit(NoKey node);
    public void Visit(NameKey node);

    public void Visit(GenericDeclaration node);
    public void Visit(GenericDeclarationParameter node);
    public void Visit(GenericParameterInfo node);

    public void Visit(ExpressionPrefix node);
    public void Visit(NamePrefix node);

    public void Visit(Assignment node);
    public void Visit(DoStatement node);
    public void Visit(LocalAssignment node);
    public void Visit(Return node);

    public void Visit(AnonymousCall node);
    public void Visit(Call node);

    public void Visit(ArrayTypeInfo node);
    public void Visit(BasicTypeInfo node);
    public void Visit(BooleanTypeInfo node);
    public void Visit(CallbackTypeInfo node);
    public void Visit(IntersectionTypeInfo node);
    public void Visit(StringTypeInfo node);
    public void Visit(TableTypeInfo node);
    public void Visit(TypeArgument node);
    public void Visit(TypeDeclaration node);
    public void Visit(TypeField node);
    public void Visit(TypeFieldKey node);
    public void Visit(UnionTypeInfo node);

    public void DefaultVisit(AstNode node);
}

public interface IAstVisitor<out T> where T : AstNode {
    public T Visit(Block node);
    public T Visit(Expression node);
    public T Visit(FunctionArgs node);
    public T Visit(FunctionBody node);
    public T Visit(Parameter node);
    public T Visit(Prefix node);
    public T Visit(Statement node);
    public T Visit(Suffix node);
    public T Visit(Var node);

    public T Visit(AnonymousFunction node);
    public T Visit(BooleanExpression node);
    public T Visit(FunctionCall node);
    public T Visit(NumberExpression node);
    public T Visit(StringExpression node);
    public T Visit(SymbolExpression node);
    public T Visit(TableConstructor node);
    public T Visit(NoKey node);
    public T Visit(NameKey node);

    public T Visit(GenericDeclaration node);
    public T Visit(GenericDeclarationParameter node);
    public T Visit(GenericParameterInfo node);

    public T Visit(ExpressionPrefix node);
    public T Visit(NamePrefix node);

    public T Visit(Assignment node);
    public T Visit(DoStatement node);
    public T Visit(LocalAssignment node);
    public T Visit(Return node);

    public T Visit(AnonymousCall node);
    public T Visit(Call node);

    public T Visit(ArrayTypeInfo node);
    public T Visit(BasicTypeInfo node);
    public T Visit(BooleanTypeInfo node);
    public T Visit(CallbackTypeInfo node);
    public T Visit(IntersectionTypeInfo node);
    public T Visit(StringTypeInfo node);
    public T Visit(TableTypeInfo node);
    public T Visit(TypeArgument node);
    public T Visit(TypeDeclaration node);
    public T Visit(TypeField node);
    public T Visit(TypeFieldKey node);
    public T Visit(UnionTypeInfo node);

    public T DefaultVisit(AstNode node);
}