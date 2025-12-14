using RobloxCS.AST.Expressions;
using RobloxCS.AST.Functions;
using RobloxCS.AST.Generics;
using RobloxCS.AST.Parameters;
using RobloxCS.AST.Prefixes;
using RobloxCS.AST.Statements;
using RobloxCS.AST.Suffixes;
using RobloxCS.AST.Types;
using Index = RobloxCS.AST.Suffixes.Index;

namespace RobloxCS.AST;

public class AstRewriter : IAstVisitor<AstNode> {
    public virtual AstNode DefaultVisit(AstNode node) => node;

    public AstNode Visit(AstNode node) => node.Accept(this);

    public virtual AstNode VisitExpression(Expression node) => node;
    public virtual AstNode VisitStatement(Statement node) => node;
    public virtual AstNode VisitPrefix(Prefix node) => node;
    public virtual AstNode VisitSuffix(Suffix node) => node;
    public virtual AstNode VisitVar(Var node) => node;
    public virtual AstNode VisitParameter(Parameter node) => node;

    public virtual AstNode VisitBlock(Block node) {
        VisitList(node.Statements);

        return node;
    }

    public virtual AstNode VisitChunk(Chunk node) {
        node.Block = Visit(node.Block);

        return node;
    }

    public virtual AstNode VisitFunctionArgs(FunctionArgs node) {
        VisitList(node.Arguments);

        return node;
    }

    public virtual AstNode VisitFunctionBody(FunctionBody node) {
        if (node.Generics is not null) VisitList(node.Generics);

        VisitList(node.Parameters);
        VisitList(node.TypeSpecifiers);

        node.ReturnType = Visit(node.ReturnType);
        node.Body = Visit(node.Body);

        return node;
    }

    public virtual AstNode VisitFunctionName(FunctionName node) => node;

    public virtual AstNode VisitAnonymousFunction(AnonymousFunctionExpression node) {
        node.Body = Visit(node.Body);

        return node;
    }

    public virtual AstNode VisitBinaryOperatorExpression(BinaryOperatorExpression node) {
        node.Left = Visit(node.Left);
        node.Right = Visit(node.Right);

        return node;
    }

    public virtual AstNode VisitBooleanExpression(BooleanExpression node) => node;
    public virtual AstNode VisitElseIfExpression(ElseIfExpression node) => throw new NotImplementedException();

    public virtual AstNode VisitFunctionCall(FunctionCallExpression node) {
        node.Prefix = Visit(node.Prefix);
        VisitList(node.Suffixes);

        return node;
    }

    public virtual AstNode VisitIfExpression(IfExpression node) => throw new NotImplementedException();
    public virtual AstNode VisitNumberExpression(NumberExpression node) => node;

    public virtual AstNode VisitParenthesisExpression(ParenthesisExpression node) {
        node.Expression = Visit(node.Expression);

        return node;
    }

    public virtual AstNode VisitStringExpression(StringExpression node) => node;
    public virtual AstNode VisitSymbolExpression(SymbolExpression node) => node;

    public virtual AstNode VisitTableConstructor(TableConstructorExpression node) {
        VisitList(node.Fields);

        return node;
    }

    public virtual AstNode VisitNoKey(NoKey node) => throw new NotImplementedException();

    public virtual AstNode VisitNameKey(NameKey node) {
        node.Value = Visit(node.Value);

        return node;
    }

    public virtual AstNode VisitTypeAssertionExpression(TypeAssertionExpression node) => throw new NotImplementedException();

    public virtual AstNode VisitUnaryOperatorExpression(UnaryOperatorExpression node) {
        node.Expression = Visit(node.Expression);

        return node;
    }

    public virtual AstNode VisitGenericDeclaration(GenericDeclaration node) => throw new NotImplementedException();
    public virtual AstNode VisitGenericDeclarationParameter(GenericDeclarationParameter node) => throw new NotImplementedException();
    public virtual AstNode VisitGenericParameterInfo(GenericParameterInfo node) => throw new NotImplementedException();
    public virtual AstNode VisitNameGenericParameter(NameGenericParameter node) => throw new NotImplementedException();
    public virtual AstNode VisitVariadicGenericParameter(VariadicGenericParameter node) => throw new NotImplementedException();
    public virtual AstNode VisitEllipsisParameter(EllipsisParameter node) => throw new NotImplementedException();

    public virtual AstNode VisitNameParameter(NameParameter node) => node;

    public virtual AstNode VisitExpressionPrefix(ExpressionPrefix node) => throw new NotImplementedException();
    public virtual AstNode VisitNamePrefix(NamePrefix node) => node;

    public virtual AstNode VisitAssignment(AssignmentStatement node) {
        VisitList(node.Vars);
        VisitList(node.Expressions);

        return node;
    }

    public virtual AstNode VisitBreakStatement(BreakStatement node) => node;

    public virtual AstNode VisitCompoundAssignmentStatement(CompoundAssignmentStatement node) {
        node.Left = Visit(node.Left);
        node.Right = Visit(node.Right);

        return node;
    }

    public virtual AstNode VisitContinueStatement(ContinueStatement node) => node;

    public virtual AstNode VisitDoStatement(DoStatement node) {
        node.Block = Visit(node.Block);

        return node;
    }

    public virtual AstNode VisitFunctionDeclaration(FunctionDeclarationStatement node) {
        node.Name = Visit(node.Name);
        node.Body = Visit(node.Body);

        return node;
    }

    public virtual AstNode VisitIfStatement(IfStatement node) {
        node.Condition = Visit(node.Condition);
        node.Block = Visit(node.Block);

        if (node.ElseIf is not null) VisitList(node.ElseIf);
        if (node.Else is not null) node.Else = Visit(node.Else);

        return node;
    }

    public virtual AstNode VisitLocalAssignment(LocalAssignmentStatement node) {
        VisitList(node.Names);
        VisitList(node.Expressions);
        VisitList(node.Types);

        return node;
    }

    public virtual AstNode VisitRepeatStatement(RepeatStatement node) {
        node.Block = Visit(node.Block);
        node.Until = Visit(node.Until);

        return node;
    }

    public virtual AstNode VisitFunctionCallStatement(FunctionCallStatement node) {
        node.Prefix = Visit(node.Prefix);
        VisitList(node.Suffixes);

        return node;
    }

    public virtual AstNode VisitReturn(ReturnStatement node) {
        VisitList(node.Returns);

        return node;
    }

    public virtual AstNode VisitWhileStatement(WhileStatement node) {
        node.Condition = Visit(node.Condition);
        node.Block = Visit(node.Block);

        return node;
    }

    public virtual AstNode VisitAnonymousCall(AnonymousCall node) {
        node.Arguments = Visit(node.Arguments);

        return node;
    }

    public virtual AstNode VisitMethodCall(MethodCall node) {
        node.Args = Visit(node.Args);

        return node;
    }
    
    public virtual AstNode VisitCall(Call node) => throw new NotImplementedException();

    public virtual AstNode VisitDot(Dot node) {
        node.Name = Visit(node.Name);

        return node;
    }

    public virtual AstNode VisitIndex(Index node) => node;

    public virtual AstNode VisitArrayTypeInfo(ArrayTypeInfo node) => throw new NotImplementedException();
    public virtual AstNode VisitBasicTypeInfo(BasicTypeInfo node) => node;
    public virtual AstNode VisitBooleanTypeInfo(BooleanTypeInfo node) => throw new NotImplementedException();

    public virtual AstNode VisitCallbackTypeInfo(CallbackTypeInfo node) {
        if (node.Generics is not null) node.Generics = Visit(node.Generics);

        VisitList(node.Arguments);
        node.ReturnType = Visit(node.ReturnType);

        return node;
    }

    public virtual AstNode VisitIntersectionTypeInfo(IntersectionTypeInfo node) => throw new NotImplementedException();
    public virtual AstNode VisitStringTypeInfo(StringTypeInfo node) => throw new NotImplementedException();

    public virtual AstNode VisitTableTypeInfo(TableTypeInfo node) {
        VisitList(node.Fields);

        return node;
    }

    public virtual AstNode VisitTypeArgument(TypeArgument node) => throw new NotImplementedException();

    public virtual AstNode VisitTypeDeclaration(TypeDeclarationStatement node) {
        if (node.Declarations is not null) VisitList(node.Declarations);
        node.DeclareAs = Visit(node.DeclareAs);

        return node;
    }

    public virtual AstNode VisitTypeField(TypeField node) {
        node.Key = Visit(node.Key);
        node.Value = Visit(node.Value);

        return node;
    }

    public virtual AstNode VisitTypeFieldKey(TypeFieldKey node) => throw new NotImplementedException();
    public virtual AstNode VisitNameTypeFieldKey(NameTypeFieldKey node) => node;
    public virtual AstNode VisitIndexSignatureFieldKey(IndexSignatureTypeFieldKey node) => throw new NotImplementedException();
    public virtual AstNode VisitTypeInfo(TypeInfo node) => throw new NotImplementedException();
    public virtual AstNode VisitUnionTypeInfo(UnionTypeInfo node) => throw new NotImplementedException();

    public virtual AstNode VisitVarExpression(VarExpression node) {
        node.Expression = Visit(node.Expression);

        return node;
    }

    public virtual AstNode VisitVarName(VarName node) => node;

    /// <summary>
    /// Visits every node in the provided list and rewrites them if required.
    /// </summary>
    protected void VisitList<T>(IList<T> list) where T : AstNode {
        for (var i = 0; i < list.Count; i++) {
            var oldNode = list[i];
            var newNode = oldNode.Accept(this);

            if (ReferenceEquals(newNode, null)) {
                list.RemoveAt(i);
                i--;

                continue;
            }

            if (ReferenceEquals(oldNode, newNode)) continue;

            if (newNode is T typedNode) {
                list[i] = typedNode;
            } else {
                throw new InvalidOperationException($"Type mismatch in list expected {typeof(T)} got {newNode.GetType().Name}.");
            }
        }
    }

    private T Visit<T>(T nodeProp) where T : AstNode => (T)nodeProp.Accept(this);
}