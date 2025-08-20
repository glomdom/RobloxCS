using RobloxCS.AST;
using RobloxCS.AST.Expressions;
using RobloxCS.AST.Parameters;
using RobloxCS.AST.Prefixes;
using RobloxCS.AST.Statements;
using RobloxCS.AST.Suffixes;
using RobloxCS.AST.Types;

namespace RobloxCS.Renderer;

public class RendererWalker : AstVisitorBase {
    private readonly RenderState _state = new();

    public string Render(Chunk chunk) {
        chunk.Accept(this);

        return _state.Builder.ToString();
    }

    public override void VisitTypeDeclaration(TypeDeclaration node) {
        _state.AppendIndent();
        _state.Builder.Append("type ");
        _state.Builder.Append(node.Name);
        _state.Builder.Append(" = ");

        node.DeclareAs.Accept(this);

        _state.Builder.AppendLine();
    }

    public override void VisitTableTypeInfo(TableTypeInfo node) {
        _state.Builder.Append('{');
        _state.Builder.AppendLine();
        _state.PushIndent();

        foreach (var field in node.Fields) {
            _state.AppendIndent();
            field.Accept(this);
            _state.Builder.AppendLine(",");
        }

        _state.PopIndent();
        _state.Builder.Append('}');
    }

    public override void VisitTypeField(TypeField node) {
        node.Key.Accept(this);
        _state.Builder.Append(": ");
        node.Value.Accept(this);
    }

    public override void VisitNameTypeFieldKey(NameTypeFieldKey node) {
        _state.Builder.Append(node.Name);
    }

    public override void VisitCallbackTypeInfo(CallbackTypeInfo node) {
        _state.Builder.Append('(');

        RenderDelimited(node.Arguments, ", ");

        _state.Builder.Append(')');
        _state.Builder.Append(" -> ");
        node.ReturnType.Accept(this);
    }

    public override void VisitBasicTypeInfo(BasicTypeInfo node) {
        _state.Builder.Append(node.Name);
    }

    public override void VisitTypeArgument(TypeArgument node) {
        _state.Builder.Append(node.Name);
        _state.Builder.Append(": ");
        node.TypeInfo.Accept(this);
    }

    public override void VisitLocalAssignment(LocalAssignment node) {
        _state.AppendIndent();
        _state.Builder.Append("local ");

        for (var i = 0; i < node.Names.Count; i++) {
            node.Names[i].Accept(this);
            _state.Builder.Append(": ");
            node.Types[i].Accept(this);

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

    public override void VisitDoStatement(DoStatement node) {
        _state.AppendIndentedLine("do");
        _state.PushIndent();

        node.Block.Accept(this);

        _state.PopIndent();
        _state.AppendIndentedLine("end");
    }

    public override void VisitAssignment(Assignment node) {
        _state.AppendIndent();
        RenderDelimited(node.Vars, ", ");
        _state.Builder.Append(" = ");
        RenderDelimited(node.Expressions, ", ");
        _state.Builder.AppendLine();
    }

    public override void VisitFunctionCall(FunctionCall node) {
        node.Prefix.Accept(this);

        RenderList(node.Suffixes);
    }

    public override void VisitNamePrefix(NamePrefix node) {
        _state.Builder.Append(node.Name);
    }

    public override void VisitAnonymousCall(AnonymousCall node) {
        _state.Builder.Append('(');

        node.Arguments.Accept(this);

        _state.Builder.Append(')');
    }

    public override void VisitFunctionArgs(FunctionArgs node) {
        RenderDelimited(node.Arguments, ", ");
    }

    public override void VisitTableConstructor(TableConstructor node) {
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
        node.Value.Accept(this);
    }

    public override void VisitAnonymousFunction(AnonymousFunction node) {
        _state.Builder.Append("function");
        node.Body.Accept(this);
        _state.AppendIndented("end");
    }

    public override void VisitFunctionBody(FunctionBody node) {
        _state.Builder.Append('(');
        RenderDelimited(node.Parameters, ", ");
        _state.Builder.Append("): ");

        node.ReturnType.Accept(this);
        _state.Builder.AppendLine();

        _state.PushIndent();
        node.Body.Accept(this);
        _state.PopIndent();
    }

    public override void VisitReturn(Return node) {
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

    public override void VisitVarName(VarName node) {
        _state.Builder.Append(node.Name);
    }

    public override void VisitSymbolExpression(SymbolExpression node) {
        _state.Builder.Append(node.Value);
    }

    public override void VisitIntersectionTypeInfo(IntersectionTypeInfo node) {
        RenderDelimited(node.Types, " & ");
    }

    private void RenderDelimited<T>(List<T> items, string delimiter) where T : AstNode {
        for (var i = 0; i < items.Count; i++) {
            items[i].Accept(this);

            if (i != items.Count - 1) {
                _state.Builder.Append(delimiter);
            }
        }
    }

    private void RenderPunctuatedLine<T>(List<T> items, string delimiter) where T : AstNode {
        foreach (var item in items) {
            item.Accept(this);

            _state.Builder.AppendLine(delimiter);
        }
    }

    private void RenderList<T>(List<T> items) where T : AstNode {
        foreach (var item in items) {
            item.Accept(this);
        }
    }
}