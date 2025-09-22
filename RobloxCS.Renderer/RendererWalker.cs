using RobloxCS.AST;
using RobloxCS.AST.Expressions;
using RobloxCS.AST.Functions;
using RobloxCS.AST.Parameters;
using RobloxCS.AST.Prefixes;
using RobloxCS.AST.Statements;
using RobloxCS.AST.Suffixes;
using RobloxCS.AST.Types;

namespace RobloxCS.Renderer;

public class RendererWalker : AstVisitorBase {
    private readonly RenderState _state = new();

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
        node.Statements.ForEach(Visit);
    }

    public override void VisitTypeAssertionExpression(TypeAssertionExpression node) {
        Visit(node.Expression);
        _state.Builder.Append(" :: ");
        Visit(node.AssertTo);
    }

    public override void VisitTypeDeclaration(TypeDeclaration node) {
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

    public override void VisitFunctionDeclaration(FunctionDeclaration node) {
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

    public override void VisitLocalAssignment(LocalAssignment node) {
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

    public override void VisitDoStatement(DoStatement node) {
        _state.AppendIndentedLine("do");
        _state.PushIndent();

        Visit(node.Block);

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
        Visit(node.Prefix);
        RenderList(node.Suffixes);
    }

    public override void VisitFunctionCallStatement(FunctionCallStatement node) {
        _state.AppendIndent();
        Visit(node.Prefix);
        RenderList(node.Suffixes);
    }

    public override void VisitNamePrefix(NamePrefix node) {
        _state.Builder.Append(node.Name);
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
        _state.Builder.AppendLine();
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
        Visit(node.Value);
    }

    public override void VisitAnonymousFunction(AnonymousFunction node) {
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
        }

        _state.Builder.Append("): ");

        Visit(node.ReturnType);
        _state.Builder.AppendLine();

        _state.PushIndent();
        Visit(node.Body);
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