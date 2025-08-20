using RobloxCS.AST.Expressions;
using RobloxCS.AST.Generics;
using RobloxCS.AST.Prefixes;
using RobloxCS.AST.Statements;
using RobloxCS.AST.Suffixes;
using RobloxCS.AST.Types;

namespace RobloxCS.AST;

public interface IAstVisitor {
    public void Visit(Block block);
    public void Visit(Expression expression);
    public void Visit(FunctionArgs args);
    public void Visit(FunctionBody body);
    public void Visit(Parameter param);
    public void Visit(Prefix prefix);
    public void Visit(Statement statement);
    public void Visit(Suffix suffix);
    public void Visit(Var var);

    public void Visit(AnonymousFunction anonFunc);
    public void Visit(BooleanExpression boolExpr);
    public void Visit(FunctionCall funcCall);
    public void Visit(NumberExpression numExpr);
    public void Visit(StringExpression strExpr);
    public void Visit(SymbolExpression symExpr);
    public void Visit(TableConstructor tableCtor);
    public void Visit(TableField tableField);
    public void Visit(NoKey noKey);
    public void Visit(NameKey nameKey);

    public void Visit(GenericDeclaration genericDecl);
    public void Visit(GenericDeclarationParameter genericDeclParam);
    public void Visit(GenericParameterInfo genericParamInfo);

    public void Visit(ExpressionPrefix exprPrefix);
    public void Visit(NamePrefix namePrefix);

    public void Visit(Assignment assignmentStmt);
    public void Visit(DoStatement doStmt);
    public void Visit(LocalAssignment localAssignmentStmt);
    public void Visit(Return returnStmt);
    
    public void Visit(AnonymousCall anonCall);
    public void Visit(Call call);
    
    public void Visit(ArrayTypeInfo arrTypeInfo);
    public void Visit(BasicTypeInfo basicTypeInfo);
    public void Visit(BooleanTypeInfo boolTypeInfo);
    public void Visit(CallbackTypeInfo callbackTypeInfo);
    public void Visit(IntersectionTypeInfo intersectionTypeInfo);
    public void Visit(StringTypeInfo strTypeInfo);
    public void Visit(TableTypeInfo tableTypeInfo);
    public void Visit(TypeArgument typeArgument);
    public void Visit(TypeDeclaration typeDeclaration);
    public void Visit(TypeField typeField);
    public void Visit(TypeFieldKey typeFieldKey);
    public void Visit(UnionTypeInfo unionTypeInfo);

    public void DefaultVisit(AstNode n);
}

public interface IAstVisitor<T> {
    public T Visit(Block block);
    public T Visit(Expression expression);
    public T Visit(FunctionArgs args);
    public T Visit(FunctionBody body);
    public T Visit(Parameter param);
    public T Visit(Prefix prefix);
    public T Visit(Statement statement);
    public T Visit(Suffix suffix);
    public T Visit(Var var);

    public T Visit(AnonymousFunction anonFunc);
    public T Visit(BooleanExpression boolExpr);
    public T Visit(FunctionCall funcCall);
    public T Visit(NumberExpression numExpr);
    public T Visit(StringExpression strExpr);
    public T Visit(SymbolExpression symExpr);
    public T Visit(TableConstructor tableCtor);
    public T Visit(NoKey noKey);
    public T Visit(NameKey nameKey);

    public T Visit(GenericDeclaration genericDecl);
    public T Visit(GenericDeclarationParameter genericDeclParam);
    public T Visit(GenericParameterInfo genericParamInfo);

    public T Visit(ExpressionPrefix exprPrefix);
    public T Visit(NamePrefix namePrefix);

    public T Visit(Assignment assignmentStmt);
    public T Visit(DoStatement doStmt);
    public T Visit(LocalAssignment localAssignmentStmt);
    public T Visit(Return returnStmt);
    
    public T Visit(AnonymousCall anonCall);
    public T Visit(Call call);
    
    public T Visit(ArrayTypeInfo arrTypeInfo);
    public T Visit(BasicTypeInfo basicTypeInfo);
    public T Visit(BooleanTypeInfo boolTypeInfo);
    public T Visit(CallbackTypeInfo callbackTypeInfo);
    public T Visit(IntersectionTypeInfo intersectionTypeInfo);
    public T Visit(StringTypeInfo strTypeInfo);
    public T Visit(TableTypeInfo tableTypeInfo);
    public T Visit(TypeArgument typeArgument);
    public T Visit(TypeDeclaration typeDeclaration);
    public T Visit(TypeField typeField);
    public T Visit(TypeFieldKey typeFieldKey);
    public T Visit(UnionTypeInfo unionTypeInfo);

    public T DefaultVisit(AstNode n);
}