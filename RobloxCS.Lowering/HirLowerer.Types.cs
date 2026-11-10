using System.Collections.Immutable;
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
    private List<Statement> LowerTopLevelType(HirType type) {
        var symbol = type.Symbol;

        if (symbol.TypeKind != TypeKind.Class) {
            Unsupported("Top-level types which aren't classes are not supported.", type.Location);

            return [];
        }

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

        var name = ClassName(symbol);
        var instanceFields = type.Fields.Where(field => !field.IsStatic).ToList();
        var staticFields = type.Fields.Where(field => field.IsStatic).ToList();

        Log.Debug(
            "Lowering {ClassKind} class {ClassName} ({InstanceFieldCount} instance fields, {StaticFieldCount} static fields, {MethodCount} methods)",
            symbol.IsStatic ? "static" : "instance", name, instanceFields.Count, staticFields.Count, type.Methods.Length
        );

        var statements = new List<Statement> { Local(name, Table()) };

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

            statements.Add(Assign(Index(name, field.Symbol.SlotName()), LowerFieldValue(field)));
        }

        if (!symbol.IsStatic) {
            statements.AddRange(LowerConstructor(type, name, instanceFields));
        }

        var seen = new HashSet<string>(StringComparer.Ordinal);

        foreach (var method in type.Methods.Where(method => !method.IsConstructor && !method.Symbol.IsAbstract)) {
            if (method.Symbol.MethodKind is not (MethodKind.Ordinary or MethodKind.PropertyGet or MethodKind.PropertySet)) {
                Unsupported($"'{method.Symbol.MethodKind}' members are not supported yet.", method.Location);

                continue;
            }

            if (!seen.Add(method.Symbol.Name)) {
                Unsupported("Method overloads are not supported yet.", method.Location);

                continue;
            }

            statements.AddRange(LowerMethod(name, method));
        }

        return statements;
    }

    private TypeInfo LowerInstanceType(INamedTypeSymbol symbol) {
        var fields = new Dictionary<string, TypeInfo>(StringComparer.Ordinal);

        for (var current = symbol; current is { SpecialType: not SpecialType.System_Object }; current = current.BaseType) {
            foreach (var field in current.GetMembers().OfType<IFieldSymbol>().Where(field => !field.IsStatic)) {
                fields.TryAdd(field.SlotName(), LowerType(field.Type, field.Locations.FirstOrDefault() ?? Location.None));
            }

            foreach (var method in current.GetMembers().OfType<IMethodSymbol>().Where(method => !method.IsStatic && method.MethodKind is MethodKind.Ordinary or MethodKind.PropertyGet or MethodKind.PropertySet)) {
                if (method.AssociatedSymbol is IPropertySymbol property && property.IsAutoProperty() && !property.IsVirtual && !property.IsOverride) {
                    continue;
                }

                fields.TryAdd(method.SlotName(), new CallbackTypeInfo {
                    Arguments = [
                        TypeArgument.From("self", Type("any")),
                        .. method.Parameters.Select(parameter => TypeArgument.From(parameter.Name, LowerType(parameter.Type, parameter.Locations.FirstOrDefault() ?? Location.None))),
                    ],
                    ReturnType = LowerType(method.ReturnType, method.Locations.FirstOrDefault() ?? Location.None),
                });
            }
        }

        // Structural instance types allow derived values wherever a base instance is expected.
        return TableType(fields.Select(field => (field.Key, field.Value)));
    }

    private List<Statement> LowerConstructor(HirType type, string name, List<HirField> instanceFields) {
        var constructors = type.Methods.Where(method => method.IsConstructor).ToList();
        if (constructors.Count > 1) {
            foreach (var extra in constructors.Skip(1)) {
                Unsupported("Constructor overloads are not supported yet.", extra.Location);
            }

            return [];
        }

        var constructor = constructors.FirstOrDefault();

        Log.Debug(
            "Lowering constructor {ClassName}.new ({ConstructorKind}, {ParameterCount} parameters)",
            name, constructor is { IsImplicit: false } ? "explicit" : "implicit", constructor?.Parameters.Length ?? 0
        );

        _names.BeginFunction();

        var parameters = constructor is null ? [] : LowerParameters(constructor.Parameters);
        var arguments = constructor is null ? new List<Expression>() : constructor.Parameters.Select(parameter => (Expression)Name(_names.NameFor(parameter.Symbol))).ToList();
        var factory = new List<Statement> {
            Local("self", Cast(new ParenthesisExpression { Expression = Cast(NewInstance(name, instanceFields.Count > 0), Type("any")) }, Type(name))),
            Stmt(Call(Index(name, "__init"), [Name("self"), .. arguments])),
            Return(Name("self")),
        };
        var factoryMethod = Function(name, "new", [.. parameters], Type(name), factory);
        parameters.Insert(0, ("self", Type("any")));
        var body = new List<Statement>();

        foreach (var field in instanceFields) {
            Log.Verbose(
                "Initializing field {ClassName}.{FieldName} from {Source}",
                name, field.Symbol.SlotName(), field.Initializer is null ? "its default value" : "its initializer"
            );

            body.AddRange(_names.Capture(() => _names.Emit(Assign(Index("self", field.Symbol.SlotName()), LowerFieldValue(field)))));
        }

        if (constructor?.Initializer is { } initializer) {
            var args = new List<Expression> { Name("self") };
            body.AddRange(_names.Capture(() => args.AddRange(initializer.Arguments.Select(argument => LowerExpression(argument.Value)))));
            body.Add(Stmt(Call(Index(ReferenceType(initializer.Method.ContainingType, initializer.Location), "__init"), args)));
        }

        if (constructor is { IsImplicit: false }) {
            _returnsSelf = true;

            body.AddRange(LowerBlock(constructor.Block));

            _returnsSelf = false;
        }

        body.Add(Return(Name("self")));

        return [Function(name, "__init", parameters, Type("any"), body), factoryMethod];
    }

    private List<Statement> LowerMethod(string className, HirMethod method) {
        if (method.Symbol.IsGenericMethod) {
            Unsupported("Generic methods are not supported yet.", method.Location);

            return [];
        }

        Log.Debug(
            "Lowering {MethodKind} method {ClassName}.{MethodName} ({ParameterCount} parameters)",
            method.IsStatic ? "static" : "instance", className, method.Symbol.Name, method.Parameters.Length
        );

        _names.BeginFunction();

        var parameters = LowerParameters(method.Parameters);
        if (!method.IsStatic) {
            parameters.Insert(0, ("self", Type(className)));
        }

        var returnType = LowerType(method.Symbol.ReturnType, method.Location);
        var body = LowerBlock(method.Block);

        Log.Verbose("Lowered {ClassName}.{MethodName} into {StatementCount} statements", className, method.Symbol.Name, body.Count);

        return [Function(className, method.Symbol.SlotName(), parameters, returnType, body)];
    }

    private List<(string Name, TypeInfo Type)> LowerParameters(ImmutableArray<HirParameter> hirParameters) {
        var parameters = new List<(string Name, TypeInfo Type)>();

        foreach (var parameter in hirParameters) {
            if (parameter.RefKind is not RefKind.None) {
                Unsupported("ref, out and in parameters are not supported yet.", parameter.Location);
            }

            if (parameter.IsParams) {
                Unsupported("params parameters are not supported yet.", parameter.Location);
            }

            var parameterName = _names.NameFor(parameter.Symbol);
            if (parameterName != parameter.Symbol.Name) {
                Log.Verbose("Renamed parameter {CSharpName} to {LuauName}", parameter.Symbol.Name, parameterName);
            }

            parameters.Add((parameterName, LowerType(parameter.Symbol.Type, parameter.Location)));
        }

        return parameters;
    }

    private static Expression NewInstance(string name, bool hasFields) {
        Expression seed = hasFields ? Cast(Table(), Type(FieldsTypeName(name))) : Table();

        return Call("setmetatable", seed, Name(name));
    }

    private static string FieldsTypeName(string className) => $"{className}Fields";

    private TypeInfo LowerFieldType(HirField field) {
        var type = LowerType(field.Symbol.Type, field.Location);
        var startsNil = field.Initializer is null && field.Symbol.Type.IsReferenceType;

        if (startsNil || field.Symbol.NullableAnnotation is NullableAnnotation.Annotated) {
            Log.Verbose("Field {FieldName} can be nil, typing it as optional", field.Symbol.Name);

            return Optional(type);
        }

        return type;
    }

    private Expression LowerFieldValue(HirField field) {
        if (field.Initializer is { } initializer) {
            return LowerExpression(initializer);
        }

        return DefaultValue(field.Symbol.Type);
    }

    private static Expression DefaultValue(ITypeSymbol type) {
        if (type.IsReferenceType || type.OriginalDefinition.SpecialType is SpecialType.System_Nullable_T) {
            return Nil();
        }

        return type.SpecialType switch {
            SpecialType.System_Boolean => Bool(false),
            SpecialType.System_Char => Str("\0"),

            _ => Num(0),
        };
    }

    private TypeInfo LowerType(ITypeSymbol type, Location location) {
        if (type is INamedTypeSymbol { OriginalDefinition.SpecialType: SpecialType.System_Nullable_T } nullable) {
            return Optional(LowerType(nullable.TypeArguments[0], location));
        }

        if (type is { IsReferenceType: true, NullableAnnotation: NullableAnnotation.Annotated }) {
            return Optional(LowerType(type.WithNullableAnnotation(NullableAnnotation.NotAnnotated), location));
        }

        switch (type.SpecialType) {
            case SpecialType.System_Void: {
                return VoidType();
            }

            case SpecialType.System_Boolean: {
                return Type("boolean");
            }

            case SpecialType.System_String:
            case SpecialType.System_Char: {
                return Type("string");
            }

            case SpecialType.System_SByte:
            case SpecialType.System_Byte:
            case SpecialType.System_Int16:
            case SpecialType.System_UInt16:
            case SpecialType.System_Int32:
            case SpecialType.System_UInt32:
            case SpecialType.System_Int64:
            case SpecialType.System_UInt64:
            case SpecialType.System_Single:
            case SpecialType.System_Double:
            case SpecialType.System_Decimal: {
                return Type("number");
            }
        }

        if (GetRobloxNative(type) is { } native) {
            return Type(native.Name);
        }

        if (IsTranspiledType(type)) {
            return Type(ReferenceType((INamedTypeSymbol)type, location));
        }
        
        if (IsRobloxEnum(type)) {
            return Type($"Enum.{type.Name}");
        }

        Unsupported($"Type '{type.ToDisplayString()}' is not supported yet.", location);

        return Type("any");
    }
}