using RobloxCS.AST;
using RobloxCS.AST.Parameters;
using RobloxCS.AST.Prefixes;
using RobloxCS.AST.Statements;
using RobloxCS.AST.Suffixes;
using RobloxCS.AST.Types;
using Serilog;

namespace RobloxCS.Transpiler.Semantics;

/// <summary>
/// This is a special rewriter. It tracks scopes and types, and should only be used
/// for non-critical structure-altering rewriters. For example, if a rewriter only
/// changes behavior, like <c>list.Count -> #list</c>, it should be a <see cref="SemanticRewriter"/>
/// as it depends on types. But, if the rewriter changes control flow and introduces
/// new scopes or variables should use a <see cref="AstRewriter"/>.
/// </summary>
public abstract class SemanticRewriter : AstRewriter {
    protected readonly Stack<Dictionary<string, TypeInfo>> ScopeStack = new();
    protected readonly GlobalRegistry Registry;

    protected SemanticRewriter(GlobalRegistry registry) {
        Registry = registry;
    }

    public override AstNode VisitBlock(Block node) {
        ScopeStack.Push([]);
        var result = base.VisitBlock(node);
        ScopeStack.Pop();

        return result;
    }

    public override AstNode VisitFunctionDeclaration(FunctionDeclarationStatement node) {
        ScopeStack.Push([]);

        for (var i = 0; i < node.Body.Parameters.Count; i++) {
            var param = node.Body.Parameters[i];
            var paramType = node.Body.TypeSpecifiers[i];

            if (param is NameParameter nameParam) {
                ScopeStack.Peek()[nameParam.Name] = paramType;

                Log.Debug("Registered name parameter {ParamName} of type {ParamType} in {FunctionName}", nameParam.Name, paramType, node.Name);
            }
        }

        if (node.Name.ColonName is not null) {
            ScopeStack.Peek()["self"] = BasicTypeInfo.FromString($"_Instance{node.Name.Names.Last()}");

            Log.Verbose("Registered self parameter of type {ParamType} in {FunctionName}", $"_Instance{node.Name.Names.Last()}", node.Name);
        }

        var result = base.VisitFunctionDeclaration(node);

        ScopeStack.Pop();

        return result;
    }

    public override AstNode VisitLocalAssignment(LocalAssignmentStatement node) {
        if (node.Types.Count == 0) return base.VisitLocalAssignment(node);

        for (var i = 0; i < node.Names.Count; i++) {
            ScopeStack.Peek()[node.Names[i].Value] = node.Types[i];

            Log.Verbose("Registered local {LocalName} as {LocalType}", node.Names[i].Value, node.Types[i]);
        }

        return base.VisitLocalAssignment(node);
    }

    protected TypeInfo? ResolveType(AstNode node) {
        if (node is VarExpression varExpr) {
            var currentType = ResolvePrefix(varExpr.Prefix);
            Log.Verbose("Resolved prefix as {CurrentPrefix} from {OldPrefix}", currentType, varExpr.Prefix);

            if (currentType is null) return null;

            foreach (var suffix in varExpr.Suffixes) {
                currentType = ResolveSuffix(currentType, suffix);
                Log.Verbose("Resolved suffix as {CurrentSuffix} from {OldSuffix}", suffix, currentType);
            }

            return currentType;
        }

        return null;
    }

    protected TypeInfo? ResolveSuffix(TypeInfo? current, Suffix suffix) {
        if (suffix is Dot dotSuffix) {
            if (current is BasicTypeInfo basicType) {
                var className = basicType.Name;
                if (className.StartsWith("_Instance")) {
                    className = className.Substring("_Instance".Length);
                }

                var memberType = Registry.GetMemberType(className, dotSuffix.Name.Value);

                Log.Debug("Resolved {Member} of class {Class} (Runtime: {Runtime}) as {Type}", dotSuffix.Name.Value, className, basicType.Name, memberType);

                return memberType;
            }
        }

        return null;
    }

    protected TypeInfo? ResolvePrefix(Prefix prefix) {
        if (prefix is NamePrefix name) {
            var resolved = ResolveName(name.Name);

            Log.Verbose("Resolved NamePrefix {NamePrefix} as {Resolved}", name.Name, resolved);

            return resolved;
        }

        if (prefix is ExpressionPrefix expr) {
            var resolved = ResolveType(expr.Expression);

            Log.Verbose("Resolved ExpressionPrefix {ExpressionPrefix} as {Resolved}", expr.Expression, resolved);

            return resolved;
        }

        return null;
    }

    protected TypeInfo? ResolveName(string name) {
        foreach (var scope in ScopeStack) {
            if (scope.TryGetValue(name, out var type)) {
                return type;
            }
        }

        return null;
    }
}