using RobloxCS.AST;
using RobloxCS.AST.Types;

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
    protected readonly Dictionary<string, TypeInfo> GlobalRegistry;

    protected SemanticRewriter(GlobalRegistry registry) { }
}