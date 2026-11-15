using System.Collections.Immutable;
using Microsoft.CodeAnalysis;
using RobloxCS.AST.Expressions;
using RobloxCS.HIR.Expressions;
using RobloxCS.Types;
using static RobloxCS.Lowering.Luau;

namespace RobloxCS.Lowering;

public sealed partial class HirLowerer {
    private const string RobloxNativeAttributeName = "RobloxCS.Types.RobloxNativeAttribute";
    private const string RobloxNameAttributeName = "RobloxCS.Types.RobloxNameAttribute";

    private static bool IsRobloxEnum(ITypeSymbol? type) {
        return type is INamedTypeSymbol { TypeKind: TypeKind.Enum, ContainingType: { Name: "Enums" } container } &&
               container.ContainingNamespace.ToDisplayString() == "RobloxCS.Types";
    }

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

    private static Expression GetService(string name) => MethodCall(Name("game"), "GetService", Str(name));

    private ImmutableArray<HirArgument> NativeArguments(ImmutableArray<HirArgument> arguments) {
        var flattened = ImmutableArray.CreateBuilder<HirArgument>(arguments.Length);

        foreach (var argument in arguments) {
            if (!argument.Symbol.IsParams) {
                flattened.Add(argument with { Value = UnwrapBox(argument.Value) });

                continue;
            }

            if (argument.Value is not HirCollectionLiteral collection || !IsExpandedParams(collection)) {
                Unsupported("Roblox APIs take params values directly. Pass them as separate arguments instead of an existing collection.", argument.Location);

                continue;
            }

            foreach (var element in collection.Elements) {
                flattened.Add(argument with { Value = UnwrapBox(element), Location = element.Location });
            }
        }

        return flattened.ToImmutable();
    }

    private static bool IsExpandedParams(HirCollectionLiteral collection) {
        if (collection.Elements.Any(element => element is HirSpread)) {
            return false;
        }

        return collection.Length is null || collection.Length is HirLiteral { Value: int length } && length == collection.Elements.Length;
    }

    private Expression LowerNativeCall(HirCall call, (string Name, RobloxNativeType Kind) native, List<Expression> args, Expression? receiver) {
        var method = call.Method;

        if (!method.IsStatic) {
            return MethodCall(receiver!, method.Name, args);
        }

        if (native.Kind == RobloxNativeType.Service) {
            return MethodCall(GetService(native.Name), method.Name, args);
        }

        return Call(Index(native.Name, method.Name), args);
    }
}