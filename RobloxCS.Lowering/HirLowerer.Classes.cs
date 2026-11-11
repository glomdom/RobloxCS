using Microsoft.CodeAnalysis;
using RobloxCS.AST.Expressions;
using RobloxCS.AST.Statements;
using RobloxCS.AST.Types;
using RobloxCS.HIR;
using RobloxCS.HIR.Declarations;
using Serilog;
using static RobloxCS.Lowering.Luau;
using TypeInfo = RobloxCS.AST.Types.TypeInfo;

namespace RobloxCS.Lowering;

public sealed partial class HirLowerer {
    private List<Statement> LowerClass(HirType type) {
        var symbol = type.Symbol;

        if (symbol.ContainingType is not null) {
            Unsupported("Nested types are not supported yet.", type.Location);

            return [];
        }

        if (symbol.IsGenericType) {
            Unsupported("Generic classes are not supported yet.", type.Location);

            return [];
        }

        if (symbol.BaseType is { SpecialType: not SpecialType.System_Object } baseType && !IsTranspiledType(baseType)) {
            Unsupported("Inheriting from external classes is not supported yet.", type.Location);

            return [];
        }

        var name = TypeName(symbol);
        var instanceFields = type.Fields.Where(field => !field.IsStatic).ToList();
        var staticFields = type.Fields.Where(field => field.IsStatic).ToList();

        Log.Debug(
            "Lowering {ClassKind} class {ClassName} ({InstanceFieldCount} instance fields, {StaticFieldCount} static fields, {MethodCount} methods)",
            symbol.IsStatic ? "static" : "instance", name, instanceFields.Count, staticFields.Count, type.Methods.Length
        );

        var statements = new List<Statement>();

        if (symbol.BaseType is { SpecialType: not SpecialType.System_Object } parent) {
            var metatable = Table();
            metatable.Fields.Add(new NameKey { Key = "__index", Value = Name(ReferenceType(parent, type.Location)) });
            statements.Add(Stmt(Call("setmetatable", Name(name), metatable)));
        }

        if (!symbol.IsStatic) {
            statements.Add(Assign(Index(name, "__index"), Name(name)));

            if (instanceFields.Count > 0) {
                Log.Verbose("Declaring {FieldsType} for {ClassName}", FieldsTypeName(name), name);

                statements.Add(TypeDecl(FieldsTypeName(name), TableType(instanceFields.Select(field => (field.Symbol.SlotName(), LowerFieldType(field))))));
            }

            statements.Add(TypeDecl(name, LowerInstanceType(symbol), true));
        }

        foreach (var field in staticFields) {
            Log.Verbose("Assigning static field {ClassName}.{FieldName}", name, field.Symbol.Name);

            statements.AddRange(_names.Capture(() => _names.Emit(Assign(Index(name, field.Symbol.SlotName()), Cast(new ParenthesisExpression { Expression = Cast(LowerFieldValue(field), Type("any")) }, LowerFieldType(field))))));
        }

        if (!symbol.IsStatic) {
            statements.AddRange(LowerConstructor(type, name, instanceFields));
        }

        var seen = new HashSet<string>(StringComparer.Ordinal);

        foreach (var method in type.Methods.Where(method => !method.IsConstructor && !method.Symbol.IsAbstract)) {
            if (method.Symbol.MethodKind is not (MethodKind.Ordinary or MethodKind.ExplicitInterfaceImplementation or MethodKind.PropertyGet or MethodKind.PropertySet)) {
                Unsupported($"'{method.Symbol.MethodKind}' members are not supported yet.", method.Location);

                continue;
            }

            if (!seen.Add(method.Symbol.SlotName())) {
                Unsupported("Method overloads are not supported yet.", method.Location);

                continue;
            }

            statements.AddRange(LowerMethod(name, method));
        }

        statements.AddRange(LowerInterfaceBindings(type));
        return statements;
    }

    private TypeInfo LowerInstanceType(INamedTypeSymbol symbol) {
        var fields = new Dictionary<string, TypeInfo>(StringComparer.Ordinal);

        for (var current = symbol; current is { SpecialType: not SpecialType.System_Object }; current = current.BaseType) {
            foreach (var field in current.GetMembers().OfType<IFieldSymbol>().Where(field => !field.IsStatic)) {
                fields.TryAdd(field.SlotName(), LowerType(field.Type, field.Locations.FirstOrDefault() ?? Location.None));
            }

            foreach (var method in current.GetMembers().OfType<IMethodSymbol>().Where(method => !method.IsStatic && method.MethodKind is MethodKind.Ordinary or MethodKind.ExplicitInterfaceImplementation or MethodKind.PropertyGet or MethodKind.PropertySet)) {
                if (method.AssociatedSymbol is IPropertySymbol property && property.IsAutoProperty() && !property.IsVirtual && !property.IsOverride) {
                    continue;
                }

                fields.TryAdd(method.SlotName(), LowerMethodType(method));
            }
        }

        foreach (var contract in symbol.AllInterfaces) {
            foreach (var method in InterfaceMethods(contract)) {
                fields.TryAdd(method.SlotName(), LowerMethodType(method));
            }
        }

        // Structural instance types allow derived values wherever a base instance is expected.
        return TableType(fields.Select(field => (field.Key, field.Value)));
    }
}
