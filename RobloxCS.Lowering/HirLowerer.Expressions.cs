using System.Globalization;
using Microsoft.CodeAnalysis;
using RobloxCS.AST;
using RobloxCS.AST.Expressions;
using RobloxCS.HIR.Expressions;
using Serilog;
using static RobloxCS.Lowering.Luau;

namespace RobloxCS.Lowering;

public sealed partial class HirLowerer {
    private Expression LowerExpression(HirExpression expression) {
        switch (expression) {
            case HirLiteral literal: {
                return LowerLiteral(literal);
            }

            case HirLocalRef local: {
                return Name(_names.NameFor(local.Symbol));
            }

            case HirParameterRef parameter: {
                return Name(_names.NameFor(parameter.Symbol));
            }

            case HirThis: {
                return Name("self");
            }

            case HirFieldAccess field: {
                return LowerFieldAccess(field);
            }

            case HirCall call: {
                return LowerCall(call) ?? (Expression)Nil();
            }

            default: {
                Unsupported($"'{expression.GetType().Name}' expressions are not supported yet.", expression.Location);

                return Nil();
            }
        }
    }

    private static Expression LowerLiteral(HirLiteral literal) {
        return literal.Value switch {
            null => Nil(),
            bool value => Bool(value),
            string value => Str(value),
            char value => Str(value.ToString()),

            _ => Num(Convert.ToDouble(literal.Value, CultureInfo.InvariantCulture)),
        };
    }

    private VarExpression LowerFieldAccess(HirFieldAccess access) {
        var field = access.Symbol;

        if (field.IsStatic) {
            Log.Verbose("Lowering static field access {ClassName}.{FieldName}", field.ContainingType.Name, field.Name);

            return Index(ClassName(field.ContainingType), field.Name);
        }

        var receiver = access.Receiver is null ? Name("self") : LowerExpression(access.Receiver);

        return Index(receiver, field.Name);
    }

    private FunctionCallExpression? LowerCall(HirCall call) {
        var method = call.Method;
        var args = call.Arguments.Select(argument => LowerExpression(argument.Value)).ToList();

        if (IsConsoleWrite(method)) {
            if (args.Count > 1) {
                Unsupported("Console format strings are not supported yet.", call.Location);

                return null;
            }

            Log.Verbose("Rewriting {Method} to print", method.ToDisplayString());

            return Call("print", args);
        }

        if (!IsModuleType(method.ContainingType)) {
            Unsupported($"Calls to '{method.ToDisplayString()}' are not supported yet.", call.Location);

            return null;
        }

        if (method.IsStatic) {
            Log.Verbose("Lowering static call {ClassName}.{MethodName} with {ArgumentCount} arguments", method.ContainingType.Name, method.Name, args.Count);

            return Call(Index(ClassName(method.ContainingType), method.Name), args);
        }

        Log.Verbose(
            "Lowering method call {MethodName} on {Receiver} with {ArgumentCount} arguments",
            method.Name, call.Receiver is null ? "implicit self" : call.Receiver.GetType().Name, args.Count
        );

        var receiver = call.Receiver is null ? Name("self") : LowerExpression(call.Receiver);

        return MethodCall(receiver, method.Name, args);
    }

    private static bool IsConsoleWrite(IMethodSymbol method) {
        return method is { Name: "WriteLine" or "Write" } && method.ContainingType.ToDisplayString() == "System.Console";
    }
}