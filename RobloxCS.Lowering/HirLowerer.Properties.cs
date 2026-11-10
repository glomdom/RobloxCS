using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.Operations;
using RobloxCS.AST;
using RobloxCS.AST.Expressions;
using RobloxCS.AST.Statements;
using RobloxCS.HIR;
using RobloxCS.HIR.Expressions;
using Serilog;
using static RobloxCS.Lowering.Luau;

namespace RobloxCS.Lowering;

public sealed partial class HirLowerer {
    private Expression LowerSourcePropertyRead(HirPropertyAccess access) {
        var property = access.Symbol;

        if (property.IsAutoProperty() && !property.IsVirtual && !property.IsOverride) {
            return SourcePropertySlot(access);
        }

        if (property.GetMethod is not { } getter) {
            Unsupported($"Property '{property.Name}' has no getter.", access.Location);

            return Nil();
        }

        Log.Verbose("Lowering read of computed property {Property} to {Getter}", property.Name, getter.SlotName());

        if (property.IsStatic) {
            return Call(Index(ReferenceType(property.ContainingType, access.Location), getter.SlotName()));
        }

        var receiver = LowerPropertyReceiver(access);
        return access.Receiver is HirThis { IsBase: true } || !getter.IsVirtual && !getter.IsOverride && !getter.IsAbstract
            ? Call(Index(ReferenceType(property.ContainingType, access.Location), getter.SlotName()), receiver)
            : MethodCall(receiver, getter.SlotName());
    }

    private VarExpression SourcePropertySlot(HirPropertyAccess access) {
        var property = access.Symbol;

        return property.IsStatic
            ? Index(ReferenceType(property.ContainingType, access.Location), property.BackingField()!.SlotName())
            : Index(LowerPropertyReceiver(access), property.BackingField()!.SlotName());
    }

    private FunctionCallExpression? LowerSetterCall(HirPropertyAccess access, HirExpression value) {
        var property = access.Symbol;
        if (property.SetMethod is not { } setter) {
            Unsupported($"Property '{property.Name}' has no setter.", access.Location);

            return null;
        }

        Log.Verbose("Lowering write of computed property {Property} to {Setter}", property.Name, setter.SlotName());

        if (property.IsStatic) {
            return Call(Index(ReferenceType(property.ContainingType, access.Location), setter.SlotName()), LowerExpression(value));
        }

        var receiver = LowerPropertyReceiver(access);

        return access.Receiver is HirThis { IsBase: true } || !setter.IsVirtual && !setter.IsOverride && !setter.IsAbstract
            ? Call(Index(ReferenceType(property.ContainingType, access.Location), setter.SlotName()), receiver, LowerExpression(value))
            : MethodCall(receiver, setter.SlotName(), LowerExpression(value));
    }

    private List<Statement> LowerComputedCompound(HirPropertyAccess access, BinaryOperatorKind op, HirExpression value, IMethodSymbol? operatorMethod) {
        Log.Verbose("Lowering compound write of computed property {Property} to getter + setter", access.Symbol.Name);

        if (access.Receiver is not { } receiver) {
            return LowerComputedCompoundOn(access, op, value, operatorMethod);
        }

        _conditionalReceivers.Push(HoistUnlessCheap(LowerExpression(receiver)));

        try {
            var target = access with {
                Receiver = new HirConditionalReceiver {
                    Type = receiver.Type,
                    Location = receiver.Location,
                },
            };

            return LowerComputedCompoundOn(target, op, value, operatorMethod);
        } finally {
            _conditionalReceivers.Pop();
        }
    }

    private List<Statement> LowerComputedCompoundOn(HirPropertyAccess target, BinaryOperatorKind op, HirExpression value, IMethodSymbol? operatorMethod) {
        var combined = new HirBinary {
            Op = op,
            Left = target,
            Right = value,
            OperatorMethod = operatorMethod,
            IsChecked = false,
            Type = target.Type,
            Location = target.Location,
        };

        if (LowerSetterCall(target, combined) is not { } call) {
            return [];
        }

        return [Stmt(call)];
    }

    private static bool IsComputedSourceProperty(HirExpression target, out HirPropertyAccess access) {
        access = null!;

        if (target is not HirPropertyAccess { Symbol: var property } candidate || !property.IsSourceProperty() || property.IsAutoProperty() && !property.IsVirtual && !property.IsOverride) {
            return false;
        }

        access = candidate;

        return true;
    }

    private Expression LowerPropertyReceiver(HirPropertyAccess access) {
        return access.Receiver is null ? Name("self") : LowerExpression(access.Receiver);
    }
}