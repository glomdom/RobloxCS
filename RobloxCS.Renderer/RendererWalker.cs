using RobloxCS.AST;
using RobloxCS.AST.Expressions;
using RobloxCS.AST.Functions;
using RobloxCS.AST.Parameters;
using RobloxCS.AST.Prefixes;
using RobloxCS.AST.Statements;
using RobloxCS.AST.Suffixes;
using RobloxCS.AST.Types;
using RobloxCS.Transpiler.Extensions;

namespace RobloxCS.Renderer;

public class RendererWalker : AstVisitorBase {
    private readonly RenderState _state = new();
    private readonly Stack<int> _precStack = new();

    public string Render(Chunk chunk) {
        Visit(chunk);

        return _state.Builder.ToString();
    }

    public override void DefaultVisit(AstNode node) {
        throw new NotImplementedException($"Node {node.GetType()} does not have a renderer.");
    }

    public override void VisitChunk(Chunk node) {
        Visit(node.Block);
    }

    public override void VisitBlock(Block node) {
        node.Statements.ForEach(VisitStatement);
    }

    public override void VisitStatement(Statement node) {
        Visit(node);
    }

    public override void VisitTypeAssertionExpression(TypeAssertionExpression node) {
        Visit(node.Expression);
        _state.Builder.Append(" :: ");
        Visit(node.AssertTo);
    }

    public override void VisitTypeDeclaration(TypeDeclarationStatement node) {
        _state.AppendIndent();
        _state.Builder.Append("type ");
        _state.Builder.Append(node.Name);
        _state.Builder.Append(" = ");

        Visit(node.DeclareAs);

        _state.Builder.AppendLine();
    }

    public override void VisitTableTypeInfo(TableTypeInfo node) {
        _state.Builder.Append('{');
        _state.Builder.AppendLine();
        _state.PushIndent();

        foreach (var field in node.Fields) {
            _state.AppendIndent();
            Visit(field);
            _state.Builder.AppendLine(",");
        }

        _state.PopIndent();
        _state.Builder.Append('}');
    }

    public override void VisitTypeField(TypeField node) {
        if (node.Access is not null) {
            _state.Builder.Append(node.Access == AccessModifier.Read ? "read " : "write ");
        }

        Visit(node.Key);
        _state.Builder.Append(": ");
        Visit(node.Value);
    }

    public override void VisitNameTypeFieldKey(NameTypeFieldKey node) {
        _state.Builder.Append(node.Name);
    }

    public override void VisitCallbackTypeInfo(CallbackTypeInfo node) {
        _state.Builder.Append('(');

        RenderDelimited(node.Arguments, ", ");

        _state.Builder.Append(')');
        _state.Builder.Append(" -> ");
        Visit(node.ReturnType);
    }

    public override void VisitBasicTypeInfo(BasicTypeInfo node) {
        _state.Builder.Append(node.Name);
    }

    public override void VisitFunctionDeclaration(FunctionDeclarationStatement node) {
        _state.AppendIndented("function ");
        _state.Builder.Append(node.Name.ToFriendly());
        Visit(node.Body);
        _state.AppendIndentedLine("end");
    }

    public override void VisitTypeArgument(TypeArgument node) {
        _state.Builder.Append(node.Name);
        _state.Builder.Append(": ");
        Visit(node.TypeInfo);
    }

    public override void VisitLocalAssignment(LocalAssignmentStatement node) {
        _state.AppendIndent();
        _state.Builder.Append("local ");

        for (var i = 0; i < node.Names.Count; i++) {
            Visit(node.Names[i]);

            if (node.Types.Count > 0) {
                _state.Builder.Append(": ");

                Visit(node.Types[i]);
            }

            if (i != node.Names.Count - 1) {
                _state.Builder.Append(", ");
            }
        }

        if (node.Expressions.Count != 0) {
            _state.Builder.Append(" = ");
            RenderDelimited(node.Expressions, ", ");
        }

        _state.Builder.AppendLine();
    }

    public override void VisitContinueStatement(ContinueStatement node) {
        _state.AppendIndentedLine("continue");
    }

    public override void VisitRepeatStatement(RepeatStatement node) {
        _state.AppendIndentedLine("repeat");

        _state.PushIndent();
        Visit(node.Block);
        _state.PopIndent();

        _state.AppendIndented("until ");
        Visit(node.Until);
        _state.Builder.AppendLine();
    }

    // TODO: Move precedence out of renderer and into a pass
    public override void VisitBinaryOperatorExpression(BinaryOperatorExpression node) {
        var parentPrec = _precStack.Count > 0 ? _precStack.Peek() : int.MaxValue;
        var prec = Precedence.Get(node.Op);
        var assocRight = Precedence.IsRightAssociative(node.Op);
        var needParens = prec < parentPrec;

        if (needParens && parentPrec != int.MaxValue) _state.Builder.Append('(');

        _precStack.Push(prec);
        Visit(node.Left);
        _precStack.Pop();

        switch (node.Op) {
            case BinOp.Minus: _state.Builder.Append(" - "); break;
            case BinOp.Plus: _state.Builder.Append(" + "); break;
            case BinOp.Star: _state.Builder.Append(" * "); break;
            case BinOp.GreaterThan: _state.Builder.Append(" > "); break;
            case BinOp.LessThan: _state.Builder.Append(" < "); break;
            case BinOp.TwoEqual: _state.Builder.Append(" == "); break;
            case BinOp.Percent: _state.Builder.Append(" % "); break;
            case BinOp.And: _state.Builder.Append(" and "); break;
            case BinOp.TildeEqual: _state.Builder.Append(" ~= "); break;

            default: throw new ArgumentOutOfRangeException(nameof(node), node.Op, null);
        }

        _precStack.Push(prec - (assocRight ? 1 : 0));
        Visit(node.Right);
        _precStack.Pop();

        if (needParens && parentPrec != int.MaxValue) _state.Builder.Append(')');
    }

    public override void VisitUnaryOperatorExpression(UnaryOperatorExpression node) {
        switch (node.UnOp) {
            case UnOp.Minus: {
                _state.Builder.Append('-');
                Visit(node.Expression);

                break;
            }

            case UnOp.Not: {
                if (node.Expression is ParenthesisExpression) {
                    _state.Builder.Append("not ");
                    Visit(node.Expression);

                    break;
                }

                _state.Builder.Append("not (");
                Visit(node.Expression);
                _state.Builder.Append(')');

                break;
            }

            default: throw new ArgumentOutOfRangeException(nameof(node), node.UnOp, "Unhandled unary operator in VisitUnaryOperatorExpression");
        }
    }

    public override void VisitParenthesisExpression(ParenthesisExpression node) {
        _state.Builder.Append('(');
        _precStack.Push(int.MaxValue);
        Visit(node.Expression);
        _precStack.Pop();
        _state.Builder.Append(')');
    }

    public override void VisitBreakStatement(BreakStatement node) {
        _state.AppendIndentedLine("break");
    }

    public override void VisitDoStatement(DoStatement node) {
        _state.AppendIndentedLine("do");
        _state.PushIndent();

        Visit(node.Block);

        _state.PopIndent();
        _state.AppendIndentedLine("end");
    }

    public override void VisitIfStatement(IfStatement node) {
        _state.AppendIndented("if ");
        Visit(node.Condition);
        _state.Builder.AppendLine(" then");

        _state.PushIndent();
        Visit(node.Block);
        _state.PopIndent();

        if (node.ElseIf is { } elseIfList) {
            foreach (var elseIf in elseIfList) {
                _state.AppendIndented("elseif ");
                Visit(elseIf.Condition);
                _state.Builder.AppendLine(" then");

                _state.PushIndent();
                Visit(elseIf.Block);
                _state.PopIndent();
            }
        }

        if (node.Else is { } @else) {
            _state.AppendIndentedLine("else");

            _state.PushIndent();
            Visit(@else);
            _state.PopIndent();
        }

        _state.AppendIndentedLine("end");
    }

    public override void VisitAssignment(AssignmentStatement node) {
        _state.AppendIndent();
        RenderDelimited(node.Vars, ", ");
        _state.Builder.Append(" = ");
        RenderDelimited(node.Expressions, ", ");
        _state.Builder.AppendLine();
    }

    public override void VisitFunctionCall(FunctionCallExpression node) {
        Visit(node.Prefix);
        RenderList(node.Suffixes);
    }

    public override void VisitFunctionCallStatement(FunctionCallStatement node) {
        _state.AppendIndent();
        Visit(node.Prefix);
        RenderList(node.Suffixes);
        _state.Builder.AppendLine();
    }

    public override void VisitWhileStatement(WhileStatement node) {
        _state.AppendIndented("while ");
        Visit(node.Condition);
        _state.Builder.AppendLine(" do");

        _state.PushIndent();
        Visit(node.Block);
        _state.PopIndent();

        _state.AppendIndentedLine("end");
    }

    public override void VisitCompoundAssignmentStatement(CompoundAssignmentStatement node) {
        _state.AppendIndent();
        Visit(node.Left);

        switch (node.Operator) {
            case CompoundOp.PlusEqual: _state.Builder.Append(" += "); break;
            case CompoundOp.MinusEqual: _state.Builder.Append(" -= "); break;

            default: throw new ArgumentOutOfRangeException(nameof(node), node.Operator, "Unhandled compound operator in VisitCompoundAssignmentStatement");
        }

        Visit(node.Right);
        _state.Builder.AppendLine();
    }

    // TODO: Visit if else chains
    public override void VisitIfExpression(IfExpression node) {
        _state.Builder.Append("if ");
        Visit(node.Condition);
        _state.Builder.Append(" then ");
        Visit(node.If);
        _state.Builder.Append(" else ");
        Visit(node.Else);
    }

    public override void VisitInterpolatedStringExpression(InterpolatedStringExpression node) {
        _state.Builder.Append('`');
        RenderList(node.Segments);
        _state.Builder.Append(node.LastString);
        _state.Builder.Append('`');
    }

    public override void VisitInterpolatedStringSegment(InterpolatedStringSegment node) {
        _state.Builder.Append(node.Literal);
        _state.Builder.Append('{');
        Visit(node.Expression);
        _state.Builder.Append('}');
    }

    public override void VisitNamePrefix(NamePrefix node) {
        _state.Builder.Append(node.Name);
    }

    public override void VisitExpressionPrefix(ExpressionPrefix node) {
        Visit(node.Expression);
    }

    public override void VisitAnonymousCall(AnonymousCall node) {
        _state.Builder.Append('(');
        Visit(node.Arguments);
        _state.Builder.Append(')');
    }

    public override void VisitMethodCall(MethodCall node) {
        _state.Builder.Append($":{node.Name}(");
        Visit(node.Args);
        _state.Builder.Append(')');
    }

    public override void VisitDot(Dot node) {
        _state.Builder.Append('.');
        Visit(node.Name);
    }

    public override void VisitFunctionArgs(FunctionArgs node) {
        RenderDelimited(node.Arguments, ", ");
    }

    public override void VisitTableConstructor(TableConstructorExpression node) {
        if (node.Fields.Count == 0) {
            _state.Builder.Append("{}");

            return;
        }

        _state.Builder.AppendLine("{");
        _state.PushIndent();

        RenderPunctuatedLine(node.Fields, ", ");

        _state.PopIndent();
        _state.AppendIndented("}");
    }

    public override void VisitNameKey(NameKey node) {
        _state.AppendIndent();
        _state.Builder.Append(node.Key);
        _state.Builder.Append(" = ");
        Visit(node.Value);
    }

    public override void VisitAnonymousFunction(AnonymousFunctionExpression node) {
        _state.Builder.Append("function");
        Visit(node.Body);
        _state.AppendIndented("end");
    }

    public override void VisitFunctionBody(FunctionBody node) {
        _state.Builder.Append('(');

        if (node.Parameters.Count != node.TypeSpecifiers.Count) {
            throw new Exception("Function body parameter and type specifier count does not match.");
        }

        for (var i = 0; i < node.Parameters.Count; i++) {
            Visit(node.Parameters[i]);
            _state.Builder.Append(": ");
            Visit(node.TypeSpecifiers[i]);

            if (i < node.Parameters.Count - 1) {
                _state.Builder.Append(", ");
            }
        }

        _state.Builder.Append("): ");

        Visit(node.ReturnType);
        _state.Builder.AppendLine();

        _state.PushIndent();
        Visit(node.Body);
        _state.PopIndent();
    }

    public override void VisitReturn(ReturnStatement node) {
        _state.AppendIndented("return ");
        RenderDelimited(node.Returns, ", ");
        _state.Builder.AppendLine();
    }

    public override void VisitStringExpression(StringExpression node) {
        _state.Builder.Append($"\"{node.Value}\"");
    }

    public override void VisitNameParameter(NameParameter node) {
        _state.Builder.Append(node.Name);
    }

    public override void VisitEllipsisParameter(EllipsisParameter node) {
        _state.Builder.Append("...");
    }

    public override void VisitVarExpression(VarExpression node) {
        Visit(node.Prefix);
        RenderList(node.Suffixes);
    }

    public override void VisitVarName(VarName node) {
        _state.Builder.Append(node.Name);
    }

    public override void VisitSymbolExpression(SymbolExpression node) {
        _state.Builder.Append(node.Value);
    }

    public override void VisitBooleanExpression(BooleanExpression node) {
        _state.Builder.Append(node.Value ? "true" : "false");
    }

    public override void VisitNumberExpression(NumberExpression node) {
        _state.Builder.Append(node.Value);
    }

    public override void VisitIntersectionTypeInfo(IntersectionTypeInfo node) {
        RenderDelimited(node.Types, " & ");
    }

    private void RenderDelimited<T>(List<T> items, string delimiter) where T : AstNode {
        for (var i = 0; i < items.Count; i++) {
            Visit(items[i]);

            if (i != items.Count - 1) {
                _state.Builder.Append(delimiter);
            }
        }
    }

    private void RenderPunctuatedLine<T>(List<T> items, string delimiter) where T : AstNode {
        foreach (var item in items) {
            Visit(item);

            _state.Builder.AppendLine(delimiter);
        }
    }

    private void RenderList<T>(List<T> items) where T : AstNode {
        items.ForEach(Visit);
    }
}