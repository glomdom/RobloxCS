using RobloxCS.AST.Expressions;
using RobloxCS.AST.Generics;
using RobloxCS.AST.Prefixes;
using RobloxCS.AST.Statements;
using RobloxCS.AST.Suffixes;
using RobloxCS.AST.Types;

namespace RobloxCS.AST;

public class AstVisitorBase : IAstVisitor {
    public virtual void DefaultVisit(AstNode node) {
        foreach (var child in node.Children()) {
            child.Accept(this);
        }
    }

    public virtual void VisitBlock(Block node) => DefaultVisit(node);
    public virtual void VisitChunk(Chunk node) => DefaultVisit(node);
    public virtual void VisitExpression(Expression node) => DefaultVisit(node);
    public virtual void VisitFunctionArgs(FunctionArgs node) => DefaultVisit(node);
    public virtual void VisitFunctionBody(FunctionBody node) => DefaultVisit(node);
    public virtual void VisitParameter(Parameter node) => DefaultVisit(node);
    public virtual void VisitPrefix(Prefix node) => DefaultVisit(node);
    public virtual void VisitStatement(Statement node) => DefaultVisit(node);
    public virtual void VisitSuffix(Suffix node) => DefaultVisit(node);
    public virtual void VisitVar(Var node) => DefaultVisit(node);

    public virtual void VisitAnonymousFunction(AnonymousFunction node) => DefaultVisit(node);
    public virtual void VisitBooleanExpression(BooleanExpression node) => DefaultVisit(node);
    public virtual void VisitFunctionCall(FunctionCall node) => DefaultVisit(node);
    public virtual void VisitNumberExpression(NumberExpression node) => DefaultVisit(node);
    public virtual void VisitStringExpression(StringExpression node) => DefaultVisit(node);
    public virtual void VisitSymbolExpression(SymbolExpression node) => DefaultVisit(node);
    public virtual void VisitTableConstructor(TableConstructor node) => DefaultVisit(node);
    public virtual void VisitTableField(TableField node) => DefaultVisit(node);
    public virtual void VisitNoKey(NoKey node) => DefaultVisit(node);
    public virtual void VisitNameKey(NameKey node) => DefaultVisit(node);

    public virtual void VisitGenericDeclaration(GenericDeclaration node) => DefaultVisit(node);
    public virtual void VisitGenericDeclarationParameter(GenericDeclarationParameter node) => DefaultVisit(node);
    public virtual void VisitGenericParameterInfo(GenericParameterInfo node) => DefaultVisit(node);
    public virtual void VisitNameGenericParameter(NameGenericParameter node) => DefaultVisit(node);
    public virtual void VisitVariadicGenericParameter(VariadicGenericParameter node) => DefaultVisit(node);

    public virtual void VisitEllipsisParameter(EllipsisParameter node) => DefaultVisit(node);
    public virtual void VisitNameParameter(NameParameter node) => DefaultVisit(node);

    public virtual void VisitExpressionPrefix(ExpressionPrefix node) => DefaultVisit(node);
    public virtual void VisitNamePrefix(NamePrefix node) => DefaultVisit(node);

    public virtual void VisitAssignment(Assignment node) => DefaultVisit(node);
    public virtual void VisitDoStatement(DoStatement node) => DefaultVisit(node);
    public virtual void VisitFunctionDeclaration(FunctionDeclaration node) => DefaultVisit(node);
    public virtual void VisitLocalAssignment(LocalAssignment node) => DefaultVisit(node);
    public virtual void VisitReturn(Return node) => DefaultVisit(node);

    public virtual void VisitAnonymousCall(AnonymousCall node) => DefaultVisit(node);
    public virtual void VisitCall(Call node) => DefaultVisit(node);

    public virtual void VisitArrayTypeInfo(ArrayTypeInfo node) => DefaultVisit(node);
    public virtual void VisitBasicTypeInfo(BasicTypeInfo node) => DefaultVisit(node);
    public virtual void VisitBooleanTypeInfo(BooleanTypeInfo node) => DefaultVisit(node);
    public virtual void VisitCallbackTypeInfo(CallbackTypeInfo node) => DefaultVisit(node);
    public virtual void VisitIntersectionTypeInfo(IntersectionTypeInfo node) => DefaultVisit(node);
    public virtual void VisitStringTypeInfo(StringTypeInfo node) => DefaultVisit(node);
    public virtual void VisitTableTypeInfo(TableTypeInfo node) => DefaultVisit(node);
    public virtual void VisitTypeArgument(TypeArgument node) => DefaultVisit(node);
    public virtual void VisitTypeDeclaration(TypeDeclaration node) => DefaultVisit(node);
    public virtual void VisitTypeField(TypeField node) => DefaultVisit(node);
    public virtual void VisitTypeFieldKey(TypeFieldKey node) => DefaultVisit(node);
    public virtual void VisitNameTypeFieldKey(NameTypeFieldKey node) => DefaultVisit(node);
    public virtual void VisitIndexSignatureFieldKey(IndexSignatureTypeFieldKey node) => DefaultVisit(node);
    public virtual void VisitUnionTypeInfo(UnionTypeInfo node) => DefaultVisit(node);

    public virtual void VisitVarExpression(VarExpression node) => DefaultVisit(node);
    public virtual void VisitVarName(VarName node) => DefaultVisit(node);
}