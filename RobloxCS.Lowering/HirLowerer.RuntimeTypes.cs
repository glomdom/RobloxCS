using Microsoft.CodeAnalysis;
using RobloxCS.HIR;
using RobloxCS.AST.Expressions;
using RobloxCS.AST.Statements;
using static RobloxCS.Lowering.Luau;

namespace RobloxCS.Lowering;

public sealed partial class HirLowerer {
    private static string RuntimeTypeKey(ITypeSymbol type) => type is INamedTypeSymbol { TypeKind: TypeKind.Interface } contract
        ? contract.InterfaceTypeName()
        : type.WithNullableAnnotation(NullableAnnotation.NotAnnotated).ToDisplayString(SymbolDisplayFormat.FullyQualifiedFormat);

    private FunctionCallStatement LowerClassMetadata(INamedTypeSymbol symbol, Location location) {
        var metadata = Table();
        metadata.Fields.Add(new NameKey { Key = "__name", Value = Str(symbol.ToDisplayString()) });

        var interfaces = Table();
        foreach (var contract in symbol.AllInterfaces) {
            interfaces.Fields.Add(new NameKey { Key = RuntimeTypeKey(contract), Value = Bool(true) });
        }

        metadata.Fields.Add(new NameKey { Key = "__interfaces", Value = interfaces });
        if (symbol.BaseType is { SpecialType: not SpecialType.System_Object } parent) {
            var name = ReferenceType(parent, location);

            metadata.Fields.Add(new NameKey { Key = "__index", Value = Name(name) });
            metadata.Fields.Add(new NameKey { Key = "__base", Value = Name(name) });
        }

        return Stmt(Call("setmetatable", Name(TypeName(symbol)), metadata));
    }
}