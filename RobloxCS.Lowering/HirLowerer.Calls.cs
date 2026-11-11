using Microsoft.CodeAnalysis;
using RobloxCS.AST.Expressions;
using RobloxCS.AST.Statements;
using RobloxCS.HIR;
using RobloxCS.HIR.Expressions;
using RobloxCS.Types;
using Serilog;
using static RobloxCS.Lowering.Luau;

namespace RobloxCS.Lowering;

public sealed partial class HirLowerer {
    private Expression LowerObjectCreation(HirObjectCreation creation) {
        var created = LowerObjectCreationValue(creation);

        if (creation.Initializers.IsEmpty) {
            return created;
        }

        var temp = _names.FreshTemp(creation.Type.Name);
        _names.Emit(Local(temp, created));

        _initializerTargets.Push(Name(temp));

        try {
            foreach (var initializer in creation.Initializers) {
                foreach (var lowered in LowerStatement(initializer)) {
                    _names.Emit(lowered);
                }
            }
        } finally {
            _initializerTargets.Pop();
        }

        return Name(temp);
    }

    private Expression LowerObjectCreationValue(HirObjectCreation creation) {
        if (TryLowerDataTypeCreation(creation, out var dataType)) {
            return dataType;
        }

        var type = creation.Constructor.ContainingType;

        if (GetRobloxNative(type) is { } native) {
            if (native.Kind == RobloxNativeType.Instance) {
                return Call(Index("Instance", "new"), Str(native.Name));
            }

            Unsupported($"Instancing roblox type '{type.ToDisplayString()}' is not supported yet.", creation.Location);
        }

        if (!IsTranspiledType(type)) {
            Unsupported($"Creating '{type.ToDisplayString()}' is not supported yet.", creation.Location);

            return Nil();
        }

        var args = creation.Arguments.Select(argument => LowerExpression(argument.Value)).ToList();

        Log.Verbose("Lowering object creation {ClassName}.new with {ArgumentCount} arguments", type.Name, args.Count);

        return Call(Index(ReferenceType(type, creation.Location), "new"), args);
    }

    private FunctionCallExpression? LowerCall(HirCall call) {
        if (TryLowerDataTypeCall(call, out var dataTypeCall)) {
            return dataTypeCall;
        }

        var method = call.Method;
        if (method.Name == "ToString" && call.Arguments.IsEmpty && call.Receiver is { } subject && IsSourceEnum(UnwrapEnumBox(subject).Type)) {
            return (FunctionCallExpression)LowerDisplayValue(subject);
        }

        if (IsConsoleWrite(method)) {
            if (call.Arguments.Length > 1) {
                Unsupported("Console format strings are not supported yet.", call.Location);

                return null;
            }

            Log.Verbose("Rewriting {Method} to print", method.ToDisplayString());

            return Call("print", call.Arguments.Select(argument => LowerDisplayValue(argument.Value)));
        }

        var args = call.Arguments.Select(argument => LowerExpression(argument.Value)).ToList();

        if (GetRobloxNative(method.ContainingType) is { } native) {
            return LowerNativeCall(call, native, args) as FunctionCallExpression;
        }

        if (!IsTranspiledType(method.ContainingType)) {
            Unsupported($"Calls to '{method.ToDisplayString()}' are not supported yet.", call.Location);

            return null;
        }

        if (method.IsStatic) {
            Log.Verbose("Lowering static call {ClassName}.{MethodName} with {ArgumentCount} arguments", method.ContainingType.Name, method.Name, args.Count);

            return Call(Index(ReferenceType(method.ContainingType, call.Location), method.SlotName()), args);
        }

        Log.Verbose(
            "Lowering method call {MethodName} on {Receiver} with {ArgumentCount} arguments",
            method.Name, call.Receiver is null ? "implicit self" : call.Receiver.GetType().Name, args.Count
        );

        var receiver = call.Receiver is null ? Name("self") : LowerExpression(call.Receiver);

        if (call.Receiver is HirThis { IsBase: true } || !method.IsVirtual && !method.IsOverride && !method.IsAbstract) {
            return Call(Index(ReferenceType(method.ContainingType, call.Location), method.SlotName()), [receiver, .. args]);
        }

        return MethodCall(receiver, method.SlotName(), args);
    }

    private static bool IsConsoleWrite(IMethodSymbol method) {
        return method is { Name: "WriteLine" or "Write" } && method.ContainingType.ToDisplayString() == "System.Console";
    }
}
