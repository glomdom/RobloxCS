using System.Text;

namespace RobloxCS.AST.Functions;

public sealed class FunctionName : AstNode {
    public required List<string> Names { get; set; }
    public string? ColonName { get; set; }

    public static FunctionName FromString(string str) {
        var colonIdx = str.LastIndexOf(':');
        string? colonName = null;
        var left = str;

        if (colonIdx >= 0) {
            colonName = str[(colonIdx + 1)..];
            left = str[..colonIdx];
        }

        var names = left.Split('.', StringSplitOptions.RemoveEmptyEntries).ToList();

        return new FunctionName {
            Names = names,
            ColonName = colonName
        };
    }


    public string ToFriendly() {
        var sb = new StringBuilder();

        for (var i = 0; i < Names.Count; i++) {
            sb.Append(Names[i]);

            if (i != Names.Count - 1) {
                sb.Append('.');
            }
        }

        sb.Append(ColonName is not null ? $":{ColonName}" : null);

        return sb.ToString();
    }

    public override FunctionName DeepClone() => new() { Names = Names.Select(n => n).ToList(), ColonName = ColonName };
    public override void Accept(IAstVisitor v) => v.VisitFunctionName(this);
    public override T Accept<T>(IAstVisitor<T> v) => v.VisitFunctionName(this);
}