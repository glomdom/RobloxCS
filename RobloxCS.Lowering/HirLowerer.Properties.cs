using RobloxCS.AST;
using RobloxCS.AST.Expressions;
using RobloxCS.HIR;
using RobloxCS.HIR.Expressions;
using Serilog;
using static RobloxCS.Lowering.Luau;

namespace RobloxCS.Lowering;

public sealed partial class HirLowerer {
    private Expression LowerSourcePropertyRead(HirPropertyAccess access) {
        var property = access.Symbol;

        if (property.IsAutoProperty()) {
            return SourcePropertySlot(access);
        }

        if (property.GetMethod is not { } getter) {
            Unsupported($"Property '{property.Name}' has no getter.", access.Location);

            return Nil();
        }

        Log.Verbose("Lowering read of computed property {Property} to {Getter}", property.Name, getter.Name);

        if (property.IsStatic) {
            return Call(Index(ClassName(property.ContainingType), getter.Name));
        }

        return MethodCall(LowerPropertyReceiver(access), getter.Name);
    }

    private VarExpression SourcePropertySlot(HirPropertyAccess access) {
        var property = access.Symbol;

        if (property.IsStatic) {
            return Index(ClassName(property.ContainingType), property.Name);
        }

        return Index(LowerPropertyReceiver(access), property.Name);
    }

    private FunctionCallExpression? LowerSetterCall(HirPropertyAccess access, HirExpression value) {
        var property = access.Symbol;
        if (property.SetMethod is not { } setter) {
            Unsupported($"Property '{property.Name}' has no setter.", access.Location);

            return null;
        }

        Log.Verbose("Lowering write of computed property {Property} to {Setter}", property.Name, setter.Name);

        if (property.IsStatic) {
            return Call(Index(ClassName(property.ContainingType), setter.Name), LowerExpression(value));
        }

        var receiver = LowerPropertyReceiver(access);

        return MethodCall(receiver, setter.Name, LowerExpression(value));
    }

    private static bool IsComputedSourceProperty(HirExpression target, out HirPropertyAccess access) {
        access = null!;

        if (target is not HirPropertyAccess { Symbol: var property } candidate || !property.IsSourceProperty() || property.IsAutoProperty()) {
            return false;
        }

        access = candidate;

        return true;
    }

    private Expression LowerPropertyReceiver(HirPropertyAccess access) {
        return access.Receiver is null ? Name("self") : LowerExpression(access.Receiver);
    }
}