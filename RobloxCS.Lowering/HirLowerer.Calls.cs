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
        if (IsList(type)) {
            return LowerListCreation(creation);
        }

        if (IsLuaTuple(type)) {
            return LowerTupleCreation(creation);
        }

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

        var args = LowerArguments(creation.Arguments);
        args.InsertRange(0, ClassDescriptorArguments(type, creation.Location));

        Log.Verbose("Lowering object creation {ClassName}.new with {ArgumentCount} arguments", type.Name, args.Count);

        return Call(Index(ReferenceType(type, creation.Location), "new"), args);
    }

    private FunctionCallExpression? LowerCall(HirCall call) {
        if (TryLowerDataTypeCall(call, out var dataTypeCall)) {
            return dataTypeCall;
        }

        var method = call.Method;
        if (method.Name == "ToString" && call.Arguments.IsEmpty && call.Receiver is { } subject && IsSourceEnum(UnwrapBox(subject).Type)) {
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

        var receiver = method.IsStatic ? null : call.Receiver is null ? Name("self") : LowerExpression(call.Receiver);
        var repeatsReceiver = receiver is not null && CollectionElement(call.Receiver?.Type ?? method.ContainingType) is not null &&
                              method.Name is "Sort" or "FindIndex" or "FindLastIndex" or "CopyTo" or "ConvertAll";

        var expandsArrayArguments = method.ContainingType.SpecialType == SpecialType.System_Array && call.Arguments.Length == 1 && method.Name is "Sort" or "Clear";
        var native = GetRobloxNative(method.ContainingType);
        var spreadsTuple = false;
        var arguments = native is not null ? NativeArguments(call.Arguments)
            : method.MethodKind == MethodKind.DelegateInvoke ? SpreadTupleArgument(call.Arguments, out spreadsTuple)
            : call.Arguments;

        var args = LowerArguments(arguments, (hasPreludes, hasEffects) => {
            if (receiver is not null && (hasPreludes || repeatsReceiver && (hasEffects || receiver is not SymbolExpression))) {
                receiver = CaptureValue(receiver, "receiver");
            }
        }, materialize: repeatsReceiver || expandsArrayArguments);

        if (method.MethodKind == MethodKind.DelegateInvoke) {
            if (!spreadsTuple && method.Parameters is [.., var last] && IsLuaTuple(last.Type) && args.Count == method.Parameters.Length) {
                args[^1] = UnpackTuple(args[^1]);
            }

            return Call(receiver!, args);
        }

        if (TryLowerCollectionCall(call, receiver, args, out var collectionCall)) {
            return collectionCall;
        }

        if (native is not null) {
            return LowerNativeCall(call, native.Value, args, receiver) as FunctionCallExpression;
        }

        if (!IsTranspiledType(method.ContainingType)) {
            Unsupported($"Calls to '{method.ToDisplayString()}' are not supported yet.", call.Location);

            return null;
        }

        args.InsertRange(0, MethodDescriptorArguments(method, call.Location));

        if (method.IsStatic) {
            Log.Verbose("Lowering static call {ClassName}.{MethodName} with {ArgumentCount} arguments", method.ContainingType.Name, method.Name, args.Count);

            args.InsertRange(0, ClassDescriptorArguments(method.ContainingType, call.Location));

            return Call(Index(ReferenceType(method.ContainingType, call.Location), method.SlotName()), args);
        }

        Log.Verbose(
            "Lowering method call {MethodName} on {Receiver} with {ArgumentCount} arguments",
            method.Name, call.Receiver is null ? "implicit self" : call.Receiver.GetType().Name, args.Count
        );

        if (
            call.Receiver is HirThis { IsBase: true } ||
            method.ContainingType.TypeKind != TypeKind.Interface && method is { IsVirtual: false, IsOverride: false, IsAbstract: false }
        ) {
            return Call(Index(ReferenceType(method.ContainingType, call.Location), method.SlotName()), [receiver!, ..args]);
        }

        return MethodCall(receiver!, method.SlotName(), args);
    }

    private static bool IsConsoleWrite(IMethodSymbol method) {
        return method is { Name: "WriteLine" or "Write" } && method.ContainingType.ToDisplayString() == "System.Console";
    }
}