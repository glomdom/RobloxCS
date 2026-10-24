using Microsoft.CodeAnalysis;
using RobloxCS.AST.Expressions;
using RobloxCS.HIR.Expressions;
using RobloxCS.Types;
using static RobloxCS.Lowering.Luau;

namespace RobloxCS.Lowering;

public sealed partial class HirLowerer {
    private const string RobloxNativeAttributeName = "RobloxCS.Types.RobloxNativeAttribute";
    private const string RobloxNameAttributeName = "RobloxCS.Types.RobloxNameAttribute";

    private static (string Name, RobloxNativeType Kind)? GetRobloxNative(ITypeSymbol type) {
        foreach (var attribute in type.GetAttributes()) {
            if (attribute.AttributeClass?.ToDisplayString() != RobloxNativeAttributeName) {
                continue;
            }

            var name = (string)attribute.ConstructorArguments[0].Value!;
            var kind = (RobloxNativeType)(int)attribute.ConstructorArguments[1].Value!;

            return (name, kind);
        }

        return null;
    }

    private static string RobloxMemberName(ISymbol member) {
        foreach (var attribute in member.GetAttributes()) {
            if (attribute.AttributeClass?.ToDisplayString() == RobloxNameAttributeName) {
                return (string)attribute.ConstructorArguments[0].Value!;
            }
        }

        return member.Name;
    }

    private static Expression GetService(string name) => MethodCall(Name("game"), "GetService", Str(name));

    private Expression LowerNativeCall(HirCall call, (string Name, RobloxNativeType Kind) native, List<Expression> args) {
        var method = call.Method;

        if (!method.IsStatic) {
            return MethodCall(LowerExpression(call.Receiver!), method.Name, args);
        }

        if (native.Kind == RobloxNativeType.Service) {
            return MethodCall(GetService(native.Name), method.Name, args);
        }

        return Call(Index(native.Name, method.Name), args);
    }
}