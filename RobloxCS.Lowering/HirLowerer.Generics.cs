using Microsoft.CodeAnalysis;
using RobloxCS.AST;
using RobloxCS.AST.Expressions;
using RobloxCS.AST.Statements;
using RobloxCS.HIR;
using RobloxCS.HIR.Declarations;
using RobloxCS.Types;
using static RobloxCS.Lowering.Luau;
using TypeInfo = RobloxCS.AST.Types.TypeInfo;

namespace RobloxCS.Lowering;

// Generics are reified: every type parameter is backed by a runtime type descriptor (see TypeInfo in the runtime).
// Methods receive descriptors for their type parameters as hidden leading parameters, instances of generic
// classes store theirs in slots, and static methods/constructors of generic classes receive the class's as parameters.
public sealed partial class HirLowerer {
    private string _typesName = "__types";
    private bool _usesTypes;

    private FunctionCallExpression TypesCall(string method, params IEnumerable<Expression> arguments) {
        _usesTypes = true;

        return Call(Index(_typesName, method), arguments);
    }

    private readonly Dictionary<string, string> _descriptorLocals = new(StringComparer.Ordinal);
    private readonly List<Statement> _descriptorStatements = [];

    // instance methods read class type arguments from self, everything else from hidden parameters
    private bool _typeArgumentsFromSelf;

    private static bool IsOpen(ITypeSymbol type) => type switch {
        ITypeParameterSymbol => true,
        IArrayTypeSymbol array => IsOpen(array.ElementType),
        INamedTypeSymbol named => named.TypeArguments.Any(IsOpen) || named.ContainingType is { } containing && IsOpen(containing),

        _ => false,
    };

    private static string HiddenName(ITypeParameterSymbol parameter) =>
        (parameter.TypeParameterKind == TypeParameterKind.Method ? "__M_" : "__T_") + parameter.Name;

    private static string TypeSlot(ITypeParameterSymbol parameter) => $"__type_{TypeName(parameter.DeclaringType!)}_{parameter.Name}";

    private static List<(string Name, TypeInfo Type)> HiddenParameters(IEnumerable<ITypeParameterSymbol> parameters) =>
        [..parameters.Select(parameter => (HiddenName(parameter), (TypeInfo)Type("any")))];

    private static IEnumerable<ITypeParameterSymbol> ClassTypeParameters(INamedTypeSymbol type) => type.OriginalDefinition.TypeParameters;

    private List<Expression> ClassDescriptorArguments(INamedTypeSymbol type, Location location) =>
        type.IsGenericType ? [..type.TypeArguments.Select(argument => TypeDescriptor(argument, location))] : [];

    private List<Expression> MethodDescriptorArguments(IMethodSymbol method, Location location) =>
        method.IsGenericMethod ? [..method.TypeArguments.Select(argument => TypeDescriptor(argument, location))] : [];

    private Expression TypeParameterDescriptor(ITypeParameterSymbol parameter) {
        if (parameter.TypeParameterKind == TypeParameterKind.Type && _typeArgumentsFromSelf) {
            return Index(Name("self"), TypeSlot(parameter));
        }

        return Name(HiddenName(parameter));
    }

    private Expression TypeDescriptor(ITypeSymbol type, Location location) {
        if (type is ITypeParameterSymbol parameter) {
            return TypeParameterDescriptor(parameter);
        }

        type = type.WithNullableAnnotation(NullableAnnotation.NotAnnotated);

        if (IsOpen(type)) {
            return TypesCall("typeInfo", RuntimeTypeKeyOf(type), DescriptorBuilder(type, location));
        }

        var key = RuntimeTypeKey(type);
        if (_descriptorLocals.TryGetValue(key, out var existing)) {
            return Name(existing);
        }

        var builder = DescriptorBuilder(type, location);
        var name = UniqueImportName("__type_" + key.Replace("global::", ""));
        _names.ReserveGlobal(name);
        _descriptorLocals[key] = name;
        _descriptorStatements.Add(Local(name, Type("any"), TypesCall("typeInfo", Str(key), builder)));

        return Name(name);
    }

    private AnonymousFunctionExpression DescriptorBuilder(ITypeSymbol type, Location location) {
        var fields = Table();
        var preludes = _names.Capture(() => {
            fields.Fields.Add(new NameKey { Key = "key", Value = RuntimeTypeKeyOf(type) });
            fields.Fields.Add(new NameKey { Key = "name", Value = DisplayNameOf(type) });
            fields.Fields.Add(new NameKey { Key = "valueType", Value = Bool(type.IsValueType) });
            fields.Fields.Add(new NameKey { Key = "default", Value = DefaultValue(type) });
            fields.Fields.Add(new NameKey { Key = "assignable", Value = AssignableTypesOf(type) });
            fields.Fields.Add(new NameKey { Key = "test", Value = DescriptorTest(type, location) });

            if (DescriptorFactory(type, location) is { } factory) {
                fields.Fields.Add(new NameKey { Key = "new", Value = factory });
            }
        });

        return Lambda([], Type("any"), [..preludes, Return(fields)]);
    }

    private AnonymousFunctionExpression DescriptorTest(ITypeSymbol type, Location location) {
        var value = Name("value");
        Expression? test = null;

        var statements = _names.Capture(() => {
            var underlying = Underlying(type);

            if (underlying.SpecialType == SpecialType.System_Object) {
                test = value;
            } else if (underlying is ITypeParameterSymbol parameter) {
                test = Call(Index(TypeParameterDescriptor(parameter), "test"), value);
            } else if (underlying.IsValueType) {
                test = LowerBoxCast(value, underlying, true);
            } else if (
                underlying.SpecialType == SpecialType.System_String || GetRobloxNative(underlying) is not null || CollectionElement(underlying) is not null ||
                underlying is INamedTypeSymbol { TypeKind: TypeKind.Class or TypeKind.Interface } named && IsTranspiledType(named)
            ) {
                test = LowerSafeCastValue(value, underlying, location);
            } else if (underlying is INamedTypeSymbol { DelegateInvokeMethod: not null }) {
                test = CheckedValue(value, Equal(Call("typeof", value), Str("function")));
            }
        });

        List<Statement> body = test is null
            ? [Stmt(Call("error", Str($"Type checks against '{type.ToDisplayString()}' are not supported yet")))]
            : [..statements, Return(test)];

        return Lambda([("value", Type("any"))], Type("any"), body);
    }

    private AnonymousFunctionExpression? DescriptorFactory(ITypeSymbol type, Location location) {
        if (type.IsValueType) {
            return Lambda([], Type("any"), [Return(DefaultValue(type))]);
        }

        if (IsList(type)) {
            return Lambda([], Type("any"), [Return(NewCollection(type, Num(0), Num(0), false, location))]);
        }

        if (
            type is INamedTypeSymbol { TypeKind: TypeKind.Class, IsAbstract: false, IsStatic: false } named && IsTranspiledType(named) &&
            named.InstanceConstructors.Any(constructor => constructor.Parameters.IsEmpty)
        ) {
            return Lambda([], Type("any"), [Return(Call(Index(ReferenceType(named, location), "new"), ClassDescriptorArguments(named, location)))]);
        }

        return null;
    }

    // matches RuntimeTypeKey for closed types, composing open ones from their arguments' descriptors at runtime
    private Expression RuntimeTypeKeyOf(ITypeSymbol type) {
        if (!IsOpen(type)) {
            return Str(RuntimeTypeKey(type));
        }

        switch (type) {
            case ITypeParameterSymbol parameter: {
                return Index(TypeParameterDescriptor(parameter), "key");
            }

            case IArrayTypeSymbol array: {
                return Concat(RuntimeTypeKeyOf(array.ElementType), Str("[]"));
            }

            case INamedTypeSymbol { OriginalDefinition.SpecialType: SpecialType.System_Nullable_T } nullable: {
                return Concat(RuntimeTypeKeyOf(nullable.TypeArguments[0]), Str("?"));
            }

            default: {
                var named = (INamedTypeSymbol)type;
                var parts = new List<Expression> { Str(QualifiedPrefix(named) + named.Name + "<") };

                for (var i = 0; i < named.TypeArguments.Length; i++) {
                    if (i > 0) parts.Add(Str(", "));
                    parts.Add(RuntimeTypeKeyOf(named.TypeArguments[i]));
                }

                parts.Add(Str(">"));

                return Concat([..parts]);
            }
        }
    }

    private static string QualifiedPrefix(INamedTypeSymbol type) {
        if (type.ContainingType is { } containing) {
            return containing.ToDisplayString(SymbolDisplayFormat.FullyQualifiedFormat) + ".";
        }

        return type.ContainingNamespace is { IsGlobalNamespace: false } ns ? $"global::{ns.ToDisplayString()}." : "global::";
    }

    // matches CollectionDisplayName for closed types
    private Expression DisplayNameOf(ITypeSymbol type) {
        if (!IsOpen(type)) {
            return Str(CollectionDisplayName(type));
        }

        switch (type) {
            case ITypeParameterSymbol parameter: {
                return Index(TypeParameterDescriptor(parameter), "name");
            }

            case IArrayTypeSymbol array: {
                return Concat(DisplayNameOf(array.ElementType), Str("[]"));
            }

            default: {
                var named = (INamedTypeSymbol)type;
                var parts = new List<Expression> { Str(named.ContainingNamespace + "." + named.MetadataName + "[") };

                for (var i = 0; i < named.TypeArguments.Length; i++) {
                    if (i > 0) parts.Add(Str(","));
                    parts.Add(DisplayNameOf(named.TypeArguments[i]));
                }

                parts.Add(Str("]"));

                return Concat([..parts]);
            }
        }
    }

    private Expression AssignableTypesOf(ITypeSymbol type) {
        if (type is ITypeParameterSymbol parameter) {
            return Index(TypeParameterDescriptor(parameter), "assignable");
        }

        var table = Table();
        var seen = new HashSet<string>(StringComparer.Ordinal);

        void Add(ITypeSymbol assignable) {
            if (IsOpen(assignable)) {
                table.Fields.Add(new IndexKey { Key = RuntimeTypeKeyOf(assignable), Value = Bool(true) });
            } else if (seen.Add(RuntimeTypeKey(assignable))) {
                table.Fields.Add(new IndexKey { Key = Str(RuntimeTypeKey(assignable)), Value = Bool(true) });
            }
        }

        if (!IsOpen(type)) {
            foreach (var key in CollectionAssignableKeys(type)) {
                if (seen.Add(key)) {
                    table.Fields.Add(new IndexKey { Key = Str(key), Value = Bool(true) });
                }
            }

            return table;
        }

        Add(type);

        if (!type.IsValueType) {
            for (var parent = type.BaseType; parent is not null; parent = parent.BaseType) {
                Add(parent);
            }

            foreach (var contract in type.AllInterfaces.Where(contract => !contract.IsGenericType)) {
                Add(contract);
            }
        }

        return table;
    }

    // instances of a constructed generic class only match when their stored type arguments do
    private Expression CheckTypeArguments(Expression value, INamedTypeSymbol target) {
        if (!target.IsGenericType || target.TypeKind != TypeKind.Class) {
            return value;
        }

        Expression? condition = null;
        var parameters = target.OriginalDefinition.TypeParameters;

        for (var i = 0; i < parameters.Length; i++) {
            var check = Equal(Index(Index(Name("value"), TypeSlot(parameters[i])), "key"), RuntimeTypeKeyOf(target.TypeArguments[i]));
            condition = condition is null ? check : Binary(condition, BinOp.And, check);
        }

        return Call(Lambda([("value", Type("any"))], Type("any"), [
            new IfStatement { Condition = Binary(Equal(Name("value"), Nil()), BinOp.Or, new UnaryOperatorExpression { UnOp = UnOp.Not, Expression = new ParenthesisExpression { Expression = condition! } }), Block = BlockOf([Return(Nil())]) },
            Return(Name("value")),
        ]), value);
    }

    private static Expression Concat(params Expression[] parts) {
        var merged = new List<Expression>();

        foreach (var part in parts) {
            if (part is StringExpression text && merged is [.., StringExpression previous]) {
                merged[^1] = Str(previous.Value + text.Value);
            } else {
                merged.Add(part);
            }
        }

        var result = merged[0];
        for (var i = 1; i < merged.Count; i++) {
            result = Binary(result, BinOp.TwoDots, merged[i]);
        }

        return result;
    }

    // static fields of generic classes live in one table per instantiation, created on first use
    private Expression StaticStorage(INamedTypeSymbol type, Location location) {
        var reference = ReferenceType(type, location);

        return type.IsGenericType ? Call(Index(reference, StaticsFunction), ClassDescriptorArguments(type, location)) : Name(reference);
    }

    private const string StaticsFunction = "__statics";
    private const string StaticsCache = "__staticsCache";

    private List<Statement> LowerGenericStatics(INamedTypeSymbol symbol, string name, List<HirField> staticFields) {
        var previous = _typeArgumentsFromSelf;
        _typeArgumentsFromSelf = false;
        _names.BeginFunction();

        try {
            var parameters = ClassTypeParameters(symbol).ToList();
            var key = Concat([..parameters.SelectMany((parameter, i) => i == 0
                ? [Index(Name(HiddenName(parameter)), "key")]
                : new Expression[] { Str(","), Index(Name(HiddenName(parameter)), "key") })]);

            var initialize = new List<Statement> {
                Assign(VarName.FromString("statics"), Table()),
                Assign(Index(Index(name, StaticsCache), Name("key")), Name("statics")),
            };

            foreach (var field in staticFields) {
                initialize.AddRange(_names.Capture(() => _names.Emit(
                    Assign(Index(Name("statics"), field.Symbol.SlotName()), Cast(new ParenthesisExpression { Expression = Cast(LowerFieldValue(field), Type("any")) }, LowerFieldType(field)))
                )));
            }

            return [
                Assign(Index(name, StaticsCache), Cast(Table(), Type("{ [string]: any }"))),
                Function(name, StaticsFunction, HiddenParameters(parameters), Type("any"), [
                    Local("key", Type("string"), key),
                    Local("statics", Type("any"), Index(Index(name, StaticsCache), Name("key"))),
                    new IfStatement { Condition = Equal(Name("statics"), Nil()), Block = BlockOf(initialize) },
                    Return(Name("statics")),
                ]),
            ];
        } finally {
            _typeArgumentsFromSelf = previous;
        }
    }
}
