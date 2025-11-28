using RobloxCS.AST;

namespace RobloxCS.Transpiler.Walkers;

public sealed class LinkerWalker {
    public TranspilationContext Ctx { get; }

    public LinkerWalker(TranspilationContext ctx) {
        Ctx = ctx;
    }

    public void LinkParents(AstNode from) {
        foreach (var kid in from.Children()) {
            kid.Parent = from;
            
            LinkParents(kid);
        }
    }
}