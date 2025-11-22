using System.Text;
using RobloxCS.AST.Functions;

namespace RobloxCS.Transpiler.Extensions;

public static class FunctionNameExtensions {
    public static string ToFriendly(this FunctionName funcName) {
        var sb = new StringBuilder();

        for (var i = 0; i < funcName.Names.Count; i++) {
            sb.Append(funcName.Names[i]);

            if (i != funcName.Names.Count - 1) {
                sb.Append('.');
            }
        }

        sb.Append(funcName.ColonName is not null ? $":{funcName.ColonName}" : null);

        return sb.ToString();
    }
}