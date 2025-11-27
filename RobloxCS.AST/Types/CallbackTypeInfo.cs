using RobloxCS.AST.Generics;

namespace RobloxCS.AST.Types;

public sealed class CallbackTypeInfo : TypeInfo {
    public GenericDeclaration? Generics;
    public required List<TypeArgument> Arguments;
    public required TypeInfo ReturnType;

    public override CallbackTypeInfo DeepClone() {
        var newGenerics = Generics?.DeepClone();
        var newArgs = Arguments.Select(arg => arg.DeepClone()).ToList();
        var newRetType = (TypeInfo)ReturnType.DeepClone();

        var cb = new CallbackTypeInfo {
            Generics = newGenerics,
            Arguments = newArgs,
            ReturnType = newRetType,
        };

        newGenerics?.Parent = cb;
        newArgs.ForEach(a => a.Parent = cb);
        newRetType.Parent = cb;

        return cb;
    }

    public override void Accept(IAstVisitor v) => v.VisitCallbackTypeInfo(this);
    public override T Accept<T>(IAstVisitor<T> v) => v.VisitCallbackTypeInfo(this);

    public override IEnumerable<AstNode> Children() {
        if (Generics is not null) yield return Generics;
        foreach (var arg in Arguments) yield return arg;

        yield return ReturnType;
    }

    public override string ToString() => $"CallbackTypeInfo(({string.Join(", ", Arguments)}) -> {ReturnType})";
}