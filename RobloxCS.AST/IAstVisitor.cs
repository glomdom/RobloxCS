using RobloxCS.AST.Expressions;
using RobloxCS.AST.Generics;
using RobloxCS.AST.Prefixes;
using RobloxCS.AST.Statements;
using RobloxCS.AST.Suffixes;
using RobloxCS.AST.Types;

namespace RobloxCS.AST;

public interface IAstVisitor {
    public void VisitBlock(Block node);
    public void VisitChunk(Chunk node);
    public void VisitExpression(Expression node);
    public void VisitFunctionArgs(FunctionArgs node);
    public void VisitFunctionBody(FunctionBody node);
    public void VisitParameter(Parameter node);
    public void VisitPrefix(Prefix node);
    public void VisitStatement(Statement node);
    public void VisitSuffix(Suffix node);
    public void VisitVar(Var node);

    public void VisitAnonymousFunction(AnonymousFunction node);
    public void VisitBooleanExpression(BooleanExpression node);
    public void VisitFunctionCall(FunctionCall node);
    public void VisitNumberExpression(NumberExpression node);
    public void VisitStringExpression(StringExpression node);
    public void VisitSymbolExpression(SymbolExpression node);
    public void VisitTableConstructor(TableConstructor node);
    public void VisitTableField(TableField node);
    public void VisitNoKey(NoKey node);
    public void VisitNameKey(NameKey node);

    public void VisitGenericDeclaration(GenericDeclaration node);
    public void VisitGenericDeclarationParameter(GenericDeclarationParameter node);
    public void VisitGenericParameterInfo(GenericParameterInfo node);
    public void VisitNameGenericParameter(NameGenericParameter node);
    public void VisitVariadicGenericParameter(VariadicGenericParameter node);

    public void VisitEllipsisParameter(EllipsisParameter node);
    public void VisitNameParameter(NameParameter node);

    public void VisitExpressionPrefix(ExpressionPrefix node);
    public void VisitNamePrefix(NamePrefix node);

    public void VisitAssignment(Assignment node);
    public void VisitDoStatement(DoStatement node);
    public void VisitFunctionDeclaration(FunctionDeclaration node);
    public void VisitLocalAssignment(LocalAssignment node);
    public void VisitReturn(Return node);

    public void VisitAnonymousCall(AnonymousCall node);
    public void VisitCall(Call node);

    public void VisitArrayTypeInfo(ArrayTypeInfo node);
    public void VisitBasicTypeInfo(BasicTypeInfo node);
    public void VisitBooleanTypeInfo(BooleanTypeInfo node);
    public void VisitCallbackTypeInfo(CallbackTypeInfo node);
    public void VisitIntersectionTypeInfo(IntersectionTypeInfo node);
    public void VisitStringTypeInfo(StringTypeInfo node);
    public void VisitTableTypeInfo(TableTypeInfo node);
    public void VisitTypeArgument(TypeArgument node);
    public void VisitTypeDeclaration(TypeDeclaration node);
    public void VisitTypeField(TypeField node);
    public void VisitTypeFieldKey(TypeFieldKey node);
    public void VisitNameTypeFieldKey(NameTypeFieldKey node);
    public void VisitIndexSignatureFieldKey(IndexSignatureTypeFieldKey node);
    public void VisitUnionTypeInfo(UnionTypeInfo node);
    
    public void VisitVarExpression(VarExpression node);
    public void VisitVarName(VarName node);

    public void DefaultVisit(AstNode node);
}

public interface IAstVisitor<out T> where T : AstNode {
    public T VisitBlock(Block node);
    public T VisitChunk(Chunk node);
    public T VisitExpression(Expression node);
    public T VisitFunctionArgs(FunctionArgs node);
    public T VisitFunctionBody(FunctionBody node);
    public T VisitParameter(Parameter node);
    public T VisitPrefix(Prefix node);
    public T VisitStatement(Statement node);
    public T VisitSuffix(Suffix node);
    public T VisitVar(Var node);

    public T VisitAnonymousFunction(AnonymousFunction node);
    public T VisitBooleanExpression(BooleanExpression node);
    public T VisitFunctionCall(FunctionCall node);
    public T VisitNumberExpression(NumberExpression node);
    public T VisitStringExpression(StringExpression node);
    public T VisitSymbolExpression(SymbolExpression node);
    public T VisitTableConstructor(TableConstructor node);
    public T VisitNoKey(NoKey node);
    public T VisitNameKey(NameKey node);

    public T VisitGenericDeclaration(GenericDeclaration node);
    public T VisitGenericDeclarationParameter(GenericDeclarationParameter node);
    public T VisitGenericParameterInfo(GenericParameterInfo node);
    public T VisitNameGenericParameter(NameGenericParameter node);
    public T VisitVariadicGenericParameter(VariadicGenericParameter node);

    public T VisitEllipsisParameter(EllipsisParameter node);
    public T VisitNameParameter(NameParameter node);

    public T VisitExpressionPrefix(ExpressionPrefix node);
    public T VisitNamePrefix(NamePrefix node);

    public T VisitAssignment(Assignment node);
    public T VisitDoStatement(DoStatement node);
    public T VisitFunctionDeclaration(FunctionDeclaration node);
    public T VisitLocalAssignment(LocalAssignment node);
    public T VisitReturn(Return node);

    public T VisitAnonymousCall(AnonymousCall node);
    public T VisitCall(Call node);

    public T VisitArrayTypeInfo(ArrayTypeInfo node);
    public T VisitBasicTypeInfo(BasicTypeInfo node);
    public T VisitBooleanTypeInfo(BooleanTypeInfo node);
    public T VisitCallbackTypeInfo(CallbackTypeInfo node);
    public T VisitIntersectionTypeInfo(IntersectionTypeInfo node);
    public T VisitStringTypeInfo(StringTypeInfo node);
    public T VisitTableTypeInfo(TableTypeInfo node);
    public T VisitTypeArgument(TypeArgument node);
    public T VisitTypeDeclaration(TypeDeclaration node);
    public T VisitTypeField(TypeField node);
    public T VisitTypeFieldKey(TypeFieldKey node);
    public T VisitNameTypeFieldKey(NameTypeFieldKey node);
    public T VisitIndexSignatureFieldKey(IndexSignatureTypeFieldKey node);
    public T VisitUnionTypeInfo(UnionTypeInfo node);
    
    public T VisitVarExpression(VarExpression node);
    public T VisitVarName(VarName node);

    public T DefaultVisit(AstNode node);
}