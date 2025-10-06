using RobloxCS.AST.Expressions;
using RobloxCS.AST.Functions;
using RobloxCS.AST.Generics;
using RobloxCS.AST.Parameters;
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

    public void Visit(AstNode node) => node.Accept(this);

    public virtual void VisitBlock(Block node) => DefaultVisit(node);
    public virtual void VisitChunk(Chunk node) => DefaultVisit(node);
    public virtual void VisitExpression(Expression node) => DefaultVisit(node);
    public virtual void VisitFunctionArgs(FunctionArgs node) => DefaultVisit(node);
    public virtual void VisitFunctionBody(FunctionBody node) => DefaultVisit(node);
    public virtual void VisitFunctionName(FunctionName node) => DefaultVisit(node);
    public virtual void VisitParameter(Parameter node) => DefaultVisit(node);
    public virtual void VisitPrefix(Prefix node) => DefaultVisit(node);
    public virtual void VisitStatement(Statement node) => DefaultVisit(node);
    public virtual void VisitSuffix(Suffix node) => DefaultVisit(node);
    public virtual void VisitVar(Var node) => DefaultVisit(node);

    public virtual void VisitAnonymousFunction(AnonymousFunctionExpression node) => DefaultVisit(node);
    public virtual void VisitBinaryOperatorExpression(BinaryOperatorExpression node) => DefaultVisit(node);
    public virtual void VisitBooleanExpression(BooleanExpression node) => DefaultVisit(node);
    public virtual void VisitIfExpression(IfExpression node) => DefaultVisit(node);
    public virtual void VisitFunctionCall(FunctionCallExpression node) => DefaultVisit(node);
    public virtual void VisitElseIfExpression(ElseIfExpression node) => DefaultVisit(node);
    public virtual void VisitNumberExpression(NumberExpression node) => DefaultVisit(node);
    public virtual void VisitStringExpression(StringExpression node) => DefaultVisit(node);
    public virtual void VisitSymbolExpression(SymbolExpression node) => DefaultVisit(node);
    public virtual void VisitTableConstructor(TableConstructorExpression node) => DefaultVisit(node);
    public virtual void VisitTableField(TableField node) => DefaultVisit(node);
    public virtual void VisitNoKey(NoKey node) => DefaultVisit(node);
    public virtual void VisitNameKey(NameKey node) => DefaultVisit(node);
    public virtual void VisitTypeAssertionExpression(TypeAssertionExpression node) => DefaultVisit(node);

    public virtual void VisitGenericDeclaration(GenericDeclaration node) => DefaultVisit(node);
    public virtual void VisitGenericDeclarationParameter(GenericDeclarationParameter node) => DefaultVisit(node);
    public virtual void VisitGenericParameterInfo(GenericParameterInfo node) => DefaultVisit(node);
    public virtual void VisitNameGenericParameter(NameGenericParameter node) => DefaultVisit(node);
    public virtual void VisitVariadicGenericParameter(VariadicGenericParameter node) => DefaultVisit(node);

    public virtual void VisitEllipsisParameter(EllipsisParameter node) => DefaultVisit(node);
    public virtual void VisitNameParameter(NameParameter node) => DefaultVisit(node);

    public virtual void VisitExpressionPrefix(ExpressionPrefix node) => DefaultVisit(node);
    public virtual void VisitNamePrefix(NamePrefix node) => DefaultVisit(node);

    public virtual void VisitAssignment(AssignmentStatement node) => DefaultVisit(node);
    public virtual void VisitBreakStatement(BreakStatement node) => DefaultVisit(node);
    public virtual void VisitCompoundAssignmentStatement(CompoundAssignmentStatement node) => DefaultVisit(node);
    public virtual void VisitContinueStatement(ContinueStatement node) => DefaultVisit(node);
    public virtual void VisitDoStatement(DoStatement node) => DefaultVisit(node);
    public virtual void VisitFunctionDeclaration(FunctionDeclarationStatement node) => DefaultVisit(node);
    public virtual void VisitIfStatement(IfStatement node) => DefaultVisit(node);
    public virtual void VisitLocalAssignment(LocalAssignmentStatement node) => DefaultVisit(node);
    public virtual void VisitFunctionCallStatement(FunctionCallStatement node) => DefaultVisit(node);
    public virtual void VisitReturn(ReturnStatement node) => DefaultVisit(node);
    public virtual void VisitWhileStatement(WhileStatement node) => DefaultVisit(node);

    public virtual void VisitAnonymousCall(AnonymousCall node) => DefaultVisit(node);
    public virtual void VisitMethodCall(MethodCall node) => DefaultVisit(node);
    public virtual void VisitCall(Call node) => DefaultVisit(node);

    public virtual void VisitArrayTypeInfo(ArrayTypeInfo node) => DefaultVisit(node);
    public virtual void VisitBasicTypeInfo(BasicTypeInfo node) => DefaultVisit(node);
    public virtual void VisitBooleanTypeInfo(BooleanTypeInfo node) => DefaultVisit(node);
    public virtual void VisitCallbackTypeInfo(CallbackTypeInfo node) => DefaultVisit(node);
    public virtual void VisitIntersectionTypeInfo(IntersectionTypeInfo node) => DefaultVisit(node);
    public virtual void VisitStringTypeInfo(StringTypeInfo node) => DefaultVisit(node);
    public virtual void VisitTableTypeInfo(TableTypeInfo node) => DefaultVisit(node);
    public virtual void VisitTypeArgument(TypeArgument node) => DefaultVisit(node);
    public virtual void VisitTypeDeclaration(TypeDeclarationStatement node) => DefaultVisit(node);
    public virtual void VisitTypeField(TypeField node) => DefaultVisit(node);
    public virtual void VisitTypeFieldKey(TypeFieldKey node) => DefaultVisit(node);
    public virtual void VisitNameTypeFieldKey(NameTypeFieldKey node) => DefaultVisit(node);
    public virtual void VisitIndexSignatureFieldKey(IndexSignatureTypeFieldKey node) => DefaultVisit(node);
    public virtual void VisitTypeInfo(TypeInfo node) => DefaultVisit(node);
    public virtual void VisitUnionTypeInfo(UnionTypeInfo node) => DefaultVisit(node);
    public virtual void VisitUnaryOperatorExpression(UnaryOperatorExpression node) => DefaultVisit(node);

    public virtual void VisitVarExpression(VarExpression node) => DefaultVisit(node);
    public virtual void VisitVarName(VarName node) => DefaultVisit(node);
}