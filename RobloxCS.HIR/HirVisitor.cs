using RobloxCS.HIR.Declarations;
using RobloxCS.HIR.Expressions;
using RobloxCS.HIR.Interpolation;
using RobloxCS.HIR.Statements;

namespace RobloxCS.HIR;

public abstract class HirVisitor {
    public virtual void VisitCompilation(HirCompilation node) {
        foreach (var module in node.Modules) VisitModule(module);
    }

    public virtual void VisitModule(HirModule node) {
        foreach (var type in node.Types) VisitType(type);
    }

    public virtual void VisitStatement(HirStatement node) {
        switch (node) {
            case HirAssignment n: VisitAssignment(n); break;
            case HirBlock n: VisitBlock(n); break;
            case HirBreak n: VisitBreak(n); break;
            case HirCompoundAssignment n: VisitCompoundAssignment(n); break;
            case HirContinue n: VisitContinue(n); break;
            case HirDoWhile n: VisitDoWhile(n); break;
            case HirErrorStatement n: VisitErrorStatement(n); break;
            case HirExpressionStatement n: VisitExpressionStatement(n); break;
            case HirFor n: VisitFor(n); break;
            case HirIf n: VisitIf(n); break;
            case HirLocalDeclaration n: VisitLocalDeclaration(n); break;
            case HirReturn n: VisitReturn(n); break;
            case HirWhile n: VisitWhile(n); break;

            default: throw new InvalidOperationException($"Unhandled statement '{node.GetType().Name}'");
        }
    }

    public virtual void VisitExpression(HirExpression node) {
        switch (node) {
            case HirBinary n: VisitBinary(n); break;
            case HirCall n: VisitCall(n); break;
            case HirCollectionLiteral n: VisitCollectionLiteral(n); break;
            case HirConditional n: VisitConditional(n); break;
            case HirConversion n: VisitConversion(n); break;
            case HirDefault n: VisitDefault(n); break;
            case HirExpressionError n: VisitExpressionError(n); break;
            case HirFieldAccess n: VisitFieldAccess(n); break;
            case HirIncrementDecrement n: VisitIncrementDecrement(n); break;
            case HirIndexAccess n: VisitIndexAccess(n); break;
            case HirInterpolatedString n: VisitInterpolatedString(n); break;
            case HirLambda n: VisitLambda(n); break;
            case HirLiteral n: VisitLiteral(n); break;
            case HirLocalRef n: VisitLocalRef(n); break;
            case HirObjectCreation n: VisitObjectCreation(n); break;
            case HirParameterRef n: VisitParameterRef(n); break;
            case HirPropertyAccess n: VisitPropertyAccess(n); break;
            case HirThis n: VisitThis(n); break;
            case HirUnary n: VisitUnary(n); break;

            default: throw new InvalidOperationException($"Unhandled expression '{node.GetType().Name}'");
        }
    }

    public virtual void VisitDeclaration(HirDeclaration node) {
        switch (node) {
            case HirField n: VisitField(n); break;
            case HirMethod n: VisitMethod(n); break;
            case HirProperty n: VisitProperty(n); break;
            case HirType n: VisitType(n); break;

            default: throw new InvalidOperationException($"Unhandled declaration '{node.GetType().Name}'");
        }
    }

    public virtual void VisitInterpolationPart(HirInterpolationPart node) {
        switch (node) {
            case HirInterpolationExpression n: VisitInterpolationExpression(n); break;
            case HirInterpolationText n: VisitInterpolationText(n); break;

            default: throw new InvalidOperationException($"Unhandled interpolation part '{node.GetType().Name}'");
        }
    }

    protected virtual void VisitAssignment(HirAssignment node) {
        VisitExpression(node.Target);
        VisitExpression(node.Value);
    }

    protected virtual void VisitBlock(HirBlock node) {
        foreach (var stmt in node.Statements) VisitStatement(stmt);
    }

    protected virtual void VisitBreak(HirBreak node) { } // leaf

    protected virtual void VisitCompoundAssignment(HirCompoundAssignment node) {
        VisitExpression(node.Target);
        VisitExpression(node.Value);
    }

    protected virtual void VisitContinue(HirContinue node) { } // leaf

    protected virtual void VisitDoWhile(HirDoWhile node) {
        VisitExpression(node.Condition);
        VisitBlock(node.Body);
    }

    protected virtual void VisitErrorStatement(HirErrorStatement node) { } // leaf

    protected virtual void VisitExpressionStatement(HirExpressionStatement node) {
        VisitExpression(node.Expression);
    }

    protected virtual void VisitFor(HirFor node) {
        foreach (var x in node.Initializers) VisitStatement(x);
        if (node.Condition is not null) VisitExpression(node.Condition);
        foreach (var x in node.Incrementors) VisitStatement(x);

        VisitBlock(node.Body);
    }

    protected virtual void VisitIf(HirIf node) {
        VisitExpression(node.Condition);
        VisitBlock(node.Then);

        if (node.Else is not null) VisitStatement(node.Else);
    }

    protected virtual void VisitLocalDeclaration(HirLocalDeclaration node) {
        foreach (var declarator in node.Declarators) VisitVariableDeclarator(declarator);
    }

    protected virtual void VisitReturn(HirReturn node) {
        if (node.Value is not null) VisitExpression(node.Value);
    }

    protected virtual void VisitWhile(HirWhile node) {
        VisitExpression(node.Condition);
        VisitBlock(node.Body);
    }

    protected virtual void VisitBinary(HirBinary node) {
        VisitExpression(node.Left);
        VisitExpression(node.Right);
    }

    protected virtual void VisitCall(HirCall node) {
        if (node.Receiver is not null) VisitExpression(node.Receiver);

        foreach (var arg in node.Arguments) VisitArgument(arg);
    }

    protected virtual void VisitCollectionLiteral(HirCollectionLiteral node) {
        foreach (var element in node.Elements) VisitExpression(element);
    }

    protected virtual void VisitConditional(HirConditional node) {
        VisitExpression(node.Condition);
        VisitExpression(node.WhenTrue);
        VisitExpression(node.WhenFalse);
    }

    protected virtual void VisitConversion(HirConversion node) {
        VisitExpression(node.Operand);
    }

    protected virtual void VisitDefault(HirDefault node) { } // leaf

    protected virtual void VisitExpressionError(HirExpressionError node) { } // leaf

    protected virtual void VisitFieldAccess(HirFieldAccess node) {
        if (node.Receiver is not null) VisitExpression(node.Receiver);
    }

    protected virtual void VisitIncrementDecrement(HirIncrementDecrement node) {
        VisitExpression(node.Target);
    }

    protected virtual void VisitIndexAccess(HirIndexAccess node) {
        VisitExpression(node.Receiver);

        foreach (var arg in node.Arguments) VisitArgument(arg);
    }

    protected virtual void VisitInterpolatedString(HirInterpolatedString node) {
        foreach (var part in node.Parts) VisitInterpolationPart(part);
    }

    protected virtual void VisitLambda(HirLambda node) {
        foreach (var param in node.Parameters) VisitParameter(param);

        VisitBlock(node.Body);
    }

    protected virtual void VisitLiteral(HirLiteral node) { } // leaf

    protected virtual void VisitLocalRef(HirLocalRef node) { } // leaf

    protected virtual void VisitObjectCreation(HirObjectCreation node) {
        foreach (var arg in node.Arguments) VisitArgument(arg);
    }

    protected virtual void VisitParameterRef(HirParameterRef node) { } // leaf

    protected virtual void VisitPropertyAccess(HirPropertyAccess node) {
        if (node.Receiver is not null) VisitExpression(node.Receiver);
    }

    protected virtual void VisitThis(HirThis node) { } // leaf

    protected virtual void VisitUnary(HirUnary node) {
        VisitExpression(node.Operand);
    }

    protected virtual void VisitField(HirField node) {
        if (node.Initializer is not null) VisitExpression(node.Initializer);
    }

    protected virtual void VisitMethod(HirMethod node) {
        foreach (var param in node.Parameters) VisitParameter(param);

        if (node.Block is not null) VisitBlock(node.Block);
    }

    protected virtual void VisitProperty(HirProperty node) {
        if (node.Initializer is not null) VisitExpression(node.Initializer);
        if (node.Getter is not null) VisitBlock(node.Getter);
        if (node.Setter is not null) VisitBlock(node.Setter);
    }

    protected virtual void VisitType(HirType node) {
        foreach (var field in node.Fields) VisitField(field);
        foreach (var property in node.Properties) VisitProperty(property);
        foreach (var method in node.Methods) VisitMethod(method);
    }

    protected virtual void VisitInterpolationExpression(HirInterpolationExpression node) {
        VisitExpression(node.Expression);
    }

    protected virtual void VisitInterpolationText(HirInterpolationText node) { } // leaf

    protected virtual void VisitArgument(HirArgument node) {
        VisitExpression(node.Value);
    }

    protected virtual void VisitParameter(HirParameter node) {
        if (node.DefaultValue is not null) VisitExpression(node.DefaultValue);
    }

    protected virtual void VisitVariableDeclarator(HirVariableDeclarator node) {
        if (node.Initializer is not null) VisitExpression(node.Initializer);
    }
}