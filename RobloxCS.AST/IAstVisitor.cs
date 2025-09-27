using RobloxCS.AST.Expressions;
using RobloxCS.AST.Functions;
using RobloxCS.AST.Generics;
using RobloxCS.AST.Parameters;
using RobloxCS.AST.Prefixes;
using RobloxCS.AST.Statements;
using RobloxCS.AST.Suffixes;
using RobloxCS.AST.Types;

namespace RobloxCS.AST;

public interface IAstVisitor {
    void VisitBlock(Block node);
    void VisitChunk(Chunk node);
    void VisitExpression(Expression node);
    void VisitFunctionArgs(FunctionArgs node);
    void VisitFunctionBody(FunctionBody node);
    void VisitFunctionName(FunctionName node);
    void VisitParameter(Parameter node);
    void VisitPrefix(Prefix node);
    void VisitStatement(Statement node);
    void VisitSuffix(Suffix node);
    void VisitVar(Var node);

    void VisitAnonymousFunction(AnonymousFunctionExpression node);
    void VisitBinaryOperatorExpression(BinaryOperatorExpression node);
    void VisitBooleanExpression(BooleanExpression node);
    void VisitFunctionCall(FunctionCallExpression node);
    void VisitNumberExpression(NumberExpression node);
    void VisitStringExpression(StringExpression node);
    void VisitSymbolExpression(SymbolExpression node);
    void VisitTableConstructor(TableConstructorExpression node);
    void VisitTableField(TableField node);
    void VisitNoKey(NoKey node);
    void VisitNameKey(NameKey node);
    void VisitTypeAssertionExpression(TypeAssertionExpression node);
    void VisitUnaryOperatorExpression(UnaryOperatorExpression node);

    void VisitGenericDeclaration(GenericDeclaration node);
    void VisitGenericDeclarationParameter(GenericDeclarationParameter node);
    void VisitGenericParameterInfo(GenericParameterInfo node);
    void VisitNameGenericParameter(NameGenericParameter node);
    void VisitVariadicGenericParameter(VariadicGenericParameter node);

    void VisitEllipsisParameter(EllipsisParameter node);
    void VisitNameParameter(NameParameter node);

    void VisitExpressionPrefix(ExpressionPrefix node);
    void VisitNamePrefix(NamePrefix node);

    void VisitAssignment(AssignmentStatement node);
    void VisitCompoundAssignmentStatement(CompoundAssignmentStatement node);
    void VisitDoStatement(DoStatement node);
    void VisitFunctionDeclaration(FunctionDeclarationStatement node);
    void VisitIfStatement(IfStatement node);
    void VisitLocalAssignment(LocalAssignmentStatement node);
    void VisitFunctionCallStatement(FunctionCallStatement node);
    void VisitReturn(ReturnStatement node);
    void VisitWhileStatement(WhileStatement node);

    void VisitAnonymousCall(AnonymousCall node);
    void VisitMethodCall(MethodCall node);
    void VisitCall(Call node);

    void VisitArrayTypeInfo(ArrayTypeInfo node);
    void VisitBasicTypeInfo(BasicTypeInfo node);
    void VisitBooleanTypeInfo(BooleanTypeInfo node);
    void VisitCallbackTypeInfo(CallbackTypeInfo node);
    void VisitIntersectionTypeInfo(IntersectionTypeInfo node);
    void VisitStringTypeInfo(StringTypeInfo node);
    void VisitTableTypeInfo(TableTypeInfo node);
    void VisitTypeArgument(TypeArgument node);
    void VisitTypeDeclaration(TypeDeclarationStatement node);
    void VisitTypeField(TypeField node);
    void VisitTypeFieldKey(TypeFieldKey node);
    void VisitNameTypeFieldKey(NameTypeFieldKey node);
    void VisitIndexSignatureFieldKey(IndexSignatureTypeFieldKey node);
    void VisitTypeInfo(TypeInfo node);
    void VisitUnionTypeInfo(UnionTypeInfo node);

    void VisitVarExpression(VarExpression node);
    void VisitVarName(VarName node);

    void DefaultVisit(AstNode node);
}

public interface IAstVisitor<out T> where T : AstNode {
    T VisitBlock(Block node);
    T VisitChunk(Chunk node);
    T VisitExpression(Expression node);
    T VisitFunctionArgs(FunctionArgs node);
    T VisitFunctionBody(FunctionBody node);
    T VisitFunctionName(FunctionName node);
    T VisitParameter(Parameter node);
    T VisitPrefix(Prefix node);
    T VisitStatement(Statement node);
    T VisitSuffix(Suffix node);
    T VisitVar(Var node);

    T VisitAnonymousFunction(AnonymousFunctionExpression node);
    T VisitBinaryOperatorExpression(BinaryOperatorExpression node);
    T VisitBooleanExpression(BooleanExpression node);
    T VisitFunctionCall(FunctionCallExpression node);
    T VisitNumberExpression(NumberExpression node);
    T VisitStringExpression(StringExpression node);
    T VisitSymbolExpression(SymbolExpression node);
    T VisitTableConstructor(TableConstructorExpression node);
    T VisitNoKey(NoKey node);
    T VisitNameKey(NameKey node);
    T VisitTypeAssertionExpression(TypeAssertionExpression node);
    T VisitUnaryOperatorExpression(UnaryOperatorExpression node);

    T VisitGenericDeclaration(GenericDeclaration node);
    T VisitGenericDeclarationParameter(GenericDeclarationParameter node);
    T VisitGenericParameterInfo(GenericParameterInfo node);
    T VisitNameGenericParameter(NameGenericParameter node);
    T VisitVariadicGenericParameter(VariadicGenericParameter node);

    T VisitEllipsisParameter(EllipsisParameter node);
    T VisitNameParameter(NameParameter node);

    T VisitExpressionPrefix(ExpressionPrefix node);
    T VisitNamePrefix(NamePrefix node);

    T VisitAssignment(AssignmentStatement node);
    T VisitCompoundAssignmentStatement(CompoundAssignmentStatement node);
    T VisitDoStatement(DoStatement node);
    T VisitFunctionDeclaration(FunctionDeclarationStatement node);
    T VisitIfStatement(IfStatement node);
    T VisitLocalAssignment(LocalAssignmentStatement node);
    T VisitReturn(ReturnStatement node);
    T VisitWhileStatement(WhileStatement node);

    T VisitAnonymousCall(AnonymousCall node);
    T VisitMethodCall(MethodCall node);
    T VisitCall(Call node);

    T VisitArrayTypeInfo(ArrayTypeInfo node);
    T VisitBasicTypeInfo(BasicTypeInfo node);
    T VisitBooleanTypeInfo(BooleanTypeInfo node);
    T VisitCallbackTypeInfo(CallbackTypeInfo node);
    T VisitIntersectionTypeInfo(IntersectionTypeInfo node);
    T VisitStringTypeInfo(StringTypeInfo node);
    T VisitTableTypeInfo(TableTypeInfo node);
    T VisitTypeArgument(TypeArgument node);
    T VisitTypeDeclaration(TypeDeclarationStatement node);
    T VisitTypeField(TypeField node);
    T VisitTypeFieldKey(TypeFieldKey node);
    T VisitNameTypeFieldKey(NameTypeFieldKey node);
    T VisitIndexSignatureFieldKey(IndexSignatureTypeFieldKey node);
    T VisitTypeInfo(TypeInfo node);
    T VisitUnionTypeInfo(UnionTypeInfo node);

    T VisitVarExpression(VarExpression node);
    T VisitVarName(VarName node);

    T DefaultVisit(AstNode node);
}