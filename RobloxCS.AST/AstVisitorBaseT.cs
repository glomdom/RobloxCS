using RobloxCS.AST.Expressions;
using RobloxCS.AST.Generics;
using RobloxCS.AST.Parameters;
using RobloxCS.AST.Prefixes;
using RobloxCS.AST.Statements;
using RobloxCS.AST.Suffixes;
using RobloxCS.AST.Types;

namespace RobloxCS.AST;

public class AstVisitorBase<T> : IAstVisitor<T> where T : AstNode {
    public virtual T DefaultVisit(AstNode node) {
        foreach (var child in node.Children()) {
            child.Accept(this);
        }

        return (T)node;
    }
    
    public T Visit(AstNode node) => node.Accept(this);

    public virtual T VisitBlock(Block node) => DefaultVisit(node);
    public virtual T VisitChunk(Chunk node) => DefaultVisit(node);
    public virtual T VisitExpression(Expression node) => DefaultVisit(node);
    public virtual T VisitFunctionArgs(FunctionArgs node) => DefaultVisit(node);
    public virtual T VisitFunctionBody(FunctionBody node) => DefaultVisit(node);
    public virtual T VisitParameter(Parameter node) => DefaultVisit(node);
    public virtual T VisitPrefix(Prefix node) => DefaultVisit(node);
    public virtual T VisitStatement(Statement node) => DefaultVisit(node);
    public virtual T VisitSuffix(Suffix node) => DefaultVisit(node);
    public virtual T VisitVar(Var node) => DefaultVisit(node);

    public virtual T VisitAnonymousFunction(AnonymousFunction node) => DefaultVisit(node);
    public virtual T VisitBooleanExpression(BooleanExpression node) => DefaultVisit(node);
    public virtual T VisitFunctionCall(FunctionCall node) => DefaultVisit(node);
    public virtual T VisitNumberExpression(NumberExpression node) => DefaultVisit(node);
    public virtual T VisitStringExpression(StringExpression node) => DefaultVisit(node);
    public virtual T VisitSymbolExpression(SymbolExpression node) => DefaultVisit(node);
    public virtual T VisitTableConstructor(TableConstructor node) => DefaultVisit(node);
    public virtual T VisitTableField(TableField node) => DefaultVisit(node);
    public virtual T VisitNoKey(NoKey node) => DefaultVisit(node);
    public virtual T VisitNameKey(NameKey node) => DefaultVisit(node);

    public virtual T VisitGenericDeclaration(GenericDeclaration node) => DefaultVisit(node);
    public virtual T VisitGenericDeclarationParameter(GenericDeclarationParameter node) => DefaultVisit(node);
    public virtual T VisitGenericParameterInfo(GenericParameterInfo node) => DefaultVisit(node);
    public virtual T VisitNameGenericParameter(NameGenericParameter node) => DefaultVisit(node);
    public virtual T VisitVariadicGenericParameter(VariadicGenericParameter node) => DefaultVisit(node);

    public virtual T VisitEllipsisParameter(EllipsisParameter node) => DefaultVisit(node);
    public virtual T VisitNameParameter(NameParameter node) => DefaultVisit(node);

    public virtual T VisitExpressionPrefix(ExpressionPrefix node) => DefaultVisit(node);
    public virtual T VisitNamePrefix(NamePrefix node) => DefaultVisit(node);

    public virtual T VisitAssignment(Assignment node) => DefaultVisit(node);
    public virtual T VisitDoStatement(DoStatement node) => DefaultVisit(node);
    public virtual T VisitFunctionDeclaration(FunctionDeclaration node) => DefaultVisit(node);
    public virtual T VisitLocalAssignment(LocalAssignment node) => DefaultVisit(node);
    public virtual T VisitReturn(Return node) => DefaultVisit(node);

    public virtual T VisitAnonymousCall(AnonymousCall node) => DefaultVisit(node);
    public virtual T VisitCall(Call node) => DefaultVisit(node);

    public virtual T VisitArrayTypeInfo(ArrayTypeInfo node) => DefaultVisit(node);
    public virtual T VisitBasicTypeInfo(BasicTypeInfo node) => DefaultVisit(node);
    public virtual T VisitBooleanTypeInfo(BooleanTypeInfo node) => DefaultVisit(node);
    public virtual T VisitCallbackTypeInfo(CallbackTypeInfo node) => DefaultVisit(node);
    public virtual T VisitIntersectionTypeInfo(IntersectionTypeInfo node) => DefaultVisit(node);
    public virtual T VisitStringTypeInfo(StringTypeInfo node) => DefaultVisit(node);
    public virtual T VisitTableTypeInfo(TableTypeInfo node) => DefaultVisit(node);
    public virtual T VisitTypeArgument(TypeArgument node) => DefaultVisit(node);
    public virtual T VisitTypeDeclaration(TypeDeclaration node) => DefaultVisit(node);
    public virtual T VisitTypeField(TypeField node) => DefaultVisit(node);
    public virtual T VisitTypeFieldKey(TypeFieldKey node) => DefaultVisit(node);
    public virtual T VisitNameTypeFieldKey(NameTypeFieldKey node) => DefaultVisit(node);
    public virtual T VisitIndexSignatureFieldKey(IndexSignatureTypeFieldKey node) => DefaultVisit(node);
    public virtual T VisitTypeInfo(TypeInfo node) => DefaultVisit(node);
    public virtual T VisitUnionTypeInfo(UnionTypeInfo node) => DefaultVisit(node);

    public virtual T VisitVarExpression(VarExpression node) => DefaultVisit(node);
    public virtual T VisitVarName(VarName node) => DefaultVisit(node);
}