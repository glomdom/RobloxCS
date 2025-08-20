using RobloxCS.AST.Expressions;
using RobloxCS.AST.Generics;
using RobloxCS.AST.Prefixes;
using RobloxCS.AST.Statements;
using RobloxCS.AST.Suffixes;
using RobloxCS.AST.Types;

namespace RobloxCS.AST;

public class AstVisitorBase<T> : IAstVisitor<T> where T : AstNode {
    public virtual T DefaultVisit(AstNode node) => null!;
    
    public virtual T Visit(Block node) => DefaultVisit(node);
    public virtual T Visit(Expression node) => DefaultVisit(node);
    public virtual T Visit(FunctionArgs node) => DefaultVisit(node);
    public virtual T Visit(FunctionBody node) => DefaultVisit(node);
    public virtual T Visit(Parameter node) => DefaultVisit(node);
    public virtual T Visit(Prefix node) => DefaultVisit(node);
    public virtual T Visit(Statement node) => DefaultVisit(node);
    public virtual T Visit(Suffix node) => DefaultVisit(node);
    public virtual T Visit(Var node) => DefaultVisit(node);
    
    public virtual T Visit(AnonymousFunction node) => DefaultVisit(node);
    public virtual T Visit(BooleanExpression node) => DefaultVisit(node);
    public virtual T Visit(FunctionCall node) => DefaultVisit(node);
    public virtual T Visit(NumberExpression node) => DefaultVisit(node);
    public virtual T Visit(StringExpression node) => DefaultVisit(node);
    public virtual T Visit(SymbolExpression node) => DefaultVisit(node);
    public virtual T Visit(TableConstructor node) => DefaultVisit(node);
    public virtual T Visit(TableField node) => DefaultVisit(node);
    public virtual T Visit(NoKey node) => DefaultVisit(node);
    public virtual T Visit(NameKey node) => DefaultVisit(node);

    public virtual T Visit(GenericDeclaration node) => DefaultVisit(node);
    public virtual T Visit(GenericDeclarationParameter node) => DefaultVisit(node);
    public virtual T Visit(GenericParameterInfo node) => DefaultVisit(node);

    public virtual T Visit(ExpressionPrefix node) => DefaultVisit(node);
    public virtual T Visit(NamePrefix node) => DefaultVisit(node);

    public virtual T Visit(Assignment node) => DefaultVisit(node);
    public virtual T Visit(DoStatement node) => DefaultVisit(node);
    public virtual T Visit(LocalAssignment node) => DefaultVisit(node);
    public virtual T Visit(Return node) => DefaultVisit(node);

    public virtual T Visit(AnonymousCall node) => DefaultVisit(node);
    public virtual T Visit(Call node) => DefaultVisit(node);

    public virtual T Visit(ArrayTypeInfo node) => DefaultVisit(node);
    public virtual T Visit(BasicTypeInfo node) => DefaultVisit(node);
    public virtual T Visit(BooleanTypeInfo node) => DefaultVisit(node);
    public virtual T Visit(CallbackTypeInfo node) => DefaultVisit(node);
    public virtual T Visit(IntersectionTypeInfo node) => DefaultVisit(node);
    public virtual T Visit(StringTypeInfo node) => DefaultVisit(node);
    public virtual T Visit(TableTypeInfo node) => DefaultVisit(node);
    public virtual T Visit(TypeArgument node) => DefaultVisit(node);
    public virtual T Visit(TypeDeclaration node) => DefaultVisit(node);
    public virtual T Visit(TypeField node) => DefaultVisit(node);
    public virtual T Visit(TypeFieldKey node) => DefaultVisit(node);
    public virtual T Visit(UnionTypeInfo node) => DefaultVisit(node);
}