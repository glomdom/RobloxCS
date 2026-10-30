using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.Operations;
using RobloxCS.AST;
using RobloxCS.AST.Expressions;
using RobloxCS.HIR.Expressions;
using RobloxNativeType = RobloxCS.Types.RobloxNativeType;
using Serilog;
using static RobloxCS.Lowering.Luau;

namespace RobloxCS.Lowering;

public sealed partial class HirLowerer {
    private static bool TryGetNative(ITypeSymbol? type, RobloxNativeType kind, out string name) {
        name = string.Empty;

        var attribute = type?.GetAttributes().FirstOrDefault(a => a.AttributeClass?.Name == "RobloxNativeAttribute");
        if (attribute is not { ConstructorArguments: [{ Value: string robloxName }, { Value: int nativeKind }] }) {
            return false;
        }

        if ((RobloxNativeType)nativeKind != kind) {
            return false;
        }

        name = robloxName;

        return true;
    }

    private static bool IsDataType(ITypeSymbol? type, out string name) {
        return TryGetNative(type, RobloxNativeType.DataType, out name);
    }

    private static string RobloxNameOf(ISymbol symbol) {
        var attribute = symbol.GetAttributes().FirstOrDefault(a => a.AttributeClass?.Name == "RobloxNameAttribute");

        return attribute?.ConstructorArguments[0].Value as string ?? symbol.Name;
    }

    private bool TryLowerDataTypeCreation(HirObjectCreation creation, out Expression result) {
        result = Nil();

        if (!IsDataType(creation.Type, out var name)) {
            return false;
        }

        Log.Verbose("Lowering datatype creation {DataType}.new", name);

        result = Call(Index(name, "new"), creation.Arguments.Select(argument => LowerExpression(argument.Value)));

        return true;
    }

    private bool TryLowerDataTypeProperty(HirPropertyAccess access, out VarExpression result) {
        result = null!;

        var property = access.Symbol;
        if (!IsDataType(property.ContainingType, out var name)) {
            return false;
        }

        var member = RobloxNameOf(property);

        if (property.IsStatic) {
            Log.Verbose("Lowering datatype static property {DataType}.{Member}", name, member);

            result = Index(name, member);

            return true;
        }

        if (access.Receiver is null) {
            return false;
        }

        result = Index(LowerExpression(access.Receiver), member);

        return true;
    }

    private bool TryLowerDataTypeCall(HirCall call, out FunctionCallExpression result) {
        result = null!;

        var method = call.Method;
        if (!IsDataType(method.ContainingType, out var name)) {
            return false;
        }

        var member = RobloxNameOf(method);
        var args = call.Arguments.Select(argument => LowerExpression(argument.Value)).ToList();

        if (method.IsStatic) {
            Log.Verbose("Lowering datatype static call {DataType}.{Member}", name, member);

            result = Call(Index(name, member), args);

            return true;
        }

        if (call.Receiver is null) {
            return false;
        }

        result = MethodCall(LowerExpression(call.Receiver), member, args);

        return true;
    }

    private bool TryLowerDataTypeOperator(HirBinary binary, out Expression result) {
        result = Nil();

        if (binary.OperatorMethod is not { } method || !IsDataType(method.ContainingType, out var name)) {
            return false;
        }

        if (ToBinOp(binary.Op) is not { } op) {
            Unsupported($"Operator '{binary.Op}' is not supported on {name}.", binary.Location);

            return true;
        }

        result = new BinaryOperatorExpression {
            Left = LowerExpression(binary.Left),
            Op = op,
            Right = LowerExpression(binary.Right),
        };

        return true;
    }

    private bool TryLowerDataTypeOperator(HirUnary unary, out Expression result) {
        result = Nil();

        if (unary.OperatorMethod is not { } method || !IsDataType(method.ContainingType, out var name)) {
            return false;
        }

        if (unary.Op is not UnaryOperatorKind.Minus) {
            Unsupported($"Operator '{unary.Op}' is not supported on {name}.", unary.Location);

            return true;
        }

        result = new UnaryOperatorExpression {
            UnOp = UnOp.Minus,
            Expression = LowerExpression(unary.Operand),
        };

        return true;
    }
}