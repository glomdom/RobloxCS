using RobloxCS.AST.Expressions;
using RobloxCS.AST.Generics;
using RobloxCS.AST.Prefixes;
using RobloxCS.AST.Statements;
using RobloxCS.AST.Suffixes;
using RobloxCS.AST.Types;

namespace RobloxCS.AST;

public class AstVisitorBase : IAstVisitor {
    public virtual void DefaultVisit(AstNode node) { }
    
    public virtual void Visit(Block node) => DefaultVisit(node);
    public virtual void Visit(Expression node) => DefaultVisit(node);
    public virtual void Visit(FunctionArgs node) => DefaultVisit(node);
    public virtual void Visit(FunctionBody node) => DefaultVisit(node);
    public virtual void Visit(Parameter node) => DefaultVisit(node);
    public virtual void Visit(Prefix node) => DefaultVisit(node);
    public virtual void Visit(Statement node) => DefaultVisit(node);
    public virtual void Visit(Suffix node) => DefaultVisit(node);
    public virtual void Visit(Var node) => DefaultVisit(node);
    
    public virtual void Visit(AnonymousFunction node) => DefaultVisit(node);
    public virtual void Visit(BooleanExpression node) => DefaultVisit(node);
    public virtual void Visit(FunctionCall node) => DefaultVisit(node);
    public virtual void Visit(NumberExpression node) => DefaultVisit(node);
    public virtual void Visit(StringExpression node) => DefaultVisit(node);
    public virtual void Visit(SymbolExpression node) => DefaultVisit(node);
    public virtual void Visit(TableConstructor node) => DefaultVisit(node);
    public virtual void Visit(TableField node) => DefaultVisit(node);
    public virtual void Visit(NoKey node) => DefaultVisit(node);
    public virtual void Visit(NameKey node) => DefaultVisit(node);

    public virtual void Visit(GenericDeclaration node) => DefaultVisit(node);
    public virtual void Visit(GenericDeclarationParameter node) => DefaultVisit(node);
    public virtual void Visit(GenericParameterInfo node) => DefaultVisit(node);

    public virtual void Visit(ExpressionPrefix node) => DefaultVisit(node);
    public virtual void Visit(NamePrefix node) => DefaultVisit(node);

    public virtual void Visit(Assignment node) => DefaultVisit(node);
    public virtual void Visit(DoStatement node) => DefaultVisit(node);
    public virtual void Visit(LocalAssignment node) => DefaultVisit(node);
    public virtual void Visit(Return node) => DefaultVisit(node);

    public virtual void Visit(AnonymousCall node) => DefaultVisit(node);
    public virtual void Visit(Call node) => DefaultVisit(node);

    public virtual void Visit(ArrayTypeInfo node) => DefaultVisit(node);
    public virtual void Visit(BasicTypeInfo node) => DefaultVisit(node);
    public virtual void Visit(BooleanTypeInfo node) => DefaultVisit(node);
    public virtual void Visit(CallbackTypeInfo node) => DefaultVisit(node);
    public virtual void Visit(IntersectionTypeInfo node) => DefaultVisit(node);
    public virtual void Visit(StringTypeInfo node) => DefaultVisit(node);
    public virtual void Visit(TableTypeInfo node) => DefaultVisit(node);
    public virtual void Visit(TypeArgument node) => DefaultVisit(node);
    public virtual void Visit(TypeDeclaration node) => DefaultVisit(node);
    public virtual void Visit(TypeField node) => DefaultVisit(node);
    public virtual void Visit(TypeFieldKey node) => DefaultVisit(node);
    public virtual void Visit(UnionTypeInfo node) => DefaultVisit(node);
}