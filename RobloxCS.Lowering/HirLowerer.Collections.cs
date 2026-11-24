using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.Operations;
using RobloxCS.AST;
using RobloxCS.AST.Expressions;
using RobloxCS.AST.Statements;
using RobloxCS.HIR.Expressions;
using static RobloxCS.Lowering.Luau;

namespace RobloxCS.Lowering;

public sealed partial class HirLowerer {
    private string _collectionsName = "__collections";
    private bool _usesCollections;
    // whether the emitted code requires any runtime module, so the runtime folder must be installed
    public bool UsesRuntime => _usesCollections || _usesTypes;

    private static bool IsList(ITypeSymbol type) =>
        type is INamedTypeSymbol { Name: "List", Arity: 1 } named && named.ContainingNamespace.ToDisplayString() == "System.Collections.Generic";

    private static ITypeSymbol? CollectionElement(ITypeSymbol type) {
        if (type is IArrayTypeSymbol { Rank: 1 } array) {
            return array.ElementType;
        }

        if (
            type is INamedTypeSymbol { Arity: 1 } named &&
            named.ContainingNamespace.ToDisplayString() == "System.Collections.Generic" &&
            named.Name is "List" or "IList" or "ICollection" or "IEnumerable" or "IReadOnlyList" or "IReadOnlyCollection"
        ) {
            return named.TypeArguments[0];
        }

        return null;
    }

    private FunctionCallExpression CollectionCall(string method, params IEnumerable<Expression> arguments) {
        _usesCollections = true;

        return Call(Index(_collectionsName, method), arguments);
    }

    private Expression CollectionMetadata(ITypeSymbol element) {
        var assignableType = Type($"{_collectionsName}.AssignableTypes");
        var assignable = new List<Statement> { Local("types", assignableType, Table()) };
        foreach (var key in IsOpen(element) ? [] : CollectionAssignableKeys(element)) {
            assignable.Add(Assign(Index(Name("types"), Str(key)), Bool(true)));
        }

        var metadata = Table();
        metadata.Fields.Add(new NameKey { Key = "__arrayName", Value = Concat(DisplayNameOf(element), Str("[]")) });
        metadata.Fields.Add(new NameKey { Key = "__listName", Value = Concat(Str("System.Collections.Generic.List`1["), DisplayNameOf(element), Str("]")) });
        metadata.Fields.Add(new NameKey { Key = "__element", Value = RuntimeTypeKeyOf(element) });
        metadata.Fields.Add(new NameKey {
            Key = "__assignable",
            Value = IsOpen(element)
                ? Cast(AssignableTypesOf(element), assignableType)
                : Call(Lambda([], assignableType, [..assignable, Return(Name("types"))])),
        });

        return metadata;
    }

    private static IEnumerable<string> CollectionAssignableKeys(ITypeSymbol element) {
        yield return RuntimeTypeKey(element);

        if (element.IsValueType) {
            yield break;
        }

        for (var parent = element.BaseType; parent is not null; parent = parent.BaseType) {
            yield return RuntimeTypeKey(parent);
        }

        foreach (var contract in element.AllInterfaces) {
            yield return RuntimeTypeKey(contract);
        }

        if (element is IArrayTypeSymbol { Rank: 1, ElementType.IsReferenceType: true } array) {
            foreach (var key in CollectionAssignableKeys(array.ElementType)) {
                yield return key + "[]";
            }
        }
    }

    private static string CollectionDisplayName(ITypeSymbol type) {
        if (type is IArrayTypeSymbol array) return CollectionDisplayName(array.ElementType) + "[]";
        if (type.SpecialType != SpecialType.None) return type.SpecialType.ToString().Replace('_', '.');
        if (type is INamedTypeSymbol { IsGenericType: true } named) {
            return named.ContainingNamespace + "." + named.MetadataName + "[" + string.Join(",", named.TypeArguments.Select(CollectionDisplayName)) + "]";
        }

        return type.ToDisplayString();
    }

    private Expression LowerCollectionSafeCast(Expression value, ITypeSymbol target) {
        var element = CollectionElement(target)!;
        var kind = target is IArrayTypeSymbol ? "array" : IsList(target) ? "list" : target.Name is "IList" or "ICollection" ? "mutableInterface" : "interface";
        var covariant = !IsList(target);

        Expression covariantElement = element is ITypeParameterSymbol parameter && covariant
            ? new UnaryOperatorExpression { UnOp = UnOp.Not, Expression = Index(TypeParameterDescriptor(parameter), "valueType") }
            : Bool(covariant && element.IsReferenceType);

        return CollectionCall("cast", value, RuntimeTypeKeyOf(element), Str(kind), covariantElement);
    }

    private Expression NewCollection(ITypeSymbol type, Expression length, Expression capacity, bool array, Location location) {
        var element = CollectionElement(type)!;
        var accepts = Binary(Equal(Name("value"), Nil()), BinOp.Or, CollectionElementCheck(Name("value"), element, location));

        return Cast(
            CollectionCall("new", Bool(array), length, DefaultValue(element), capacity,
                CollectionMetadata(element), Lambda([("value", Type("unknown"))], Type("boolean"), [Return(accepts)])
            ), LowerType(type, location)
        );
    }

    // Roblox APIs typed as returning List<T> or T[] hand back plain Luau arrays; wrap them so the C# collection
    // operations (foreach, Count, indexing, ...) work on them.
    private FunctionCallExpression WrapNativeCollection(FunctionCallExpression nativeValue, ITypeSymbol type, Location location) {
        var element = CollectionElement(type)!;
        var accepts = Binary(Equal(Name("value"), Nil()), BinOp.Or, CollectionElementCheck(Name("value"), element, location));

        return CollectionCall("fromNative", nativeValue, Bool(type is IArrayTypeSymbol), DefaultValue(element),
            CollectionMetadata(element), Lambda([("value", Type("unknown"))], Type("boolean"), [Return(accepts)])
        );
    }

    private Expression CollectionElementCheck(Expression value, ITypeSymbol element, Location location) {
        if (element is ITypeParameterSymbol parameter) {
            return NotEqual(Call(Index(TypeParameterDescriptor(parameter), "test"), value), Nil());
        }

        if (element.SpecialType == SpecialType.System_String) {
            return Equal(Call("typeof", value), Str("string"));
        }

        if (GetRobloxNative(element) is { } native) {
            return native.Kind is RobloxCS.Types.RobloxNativeType.Instance or RobloxCS.Types.RobloxNativeType.Service
                ? Binary(Equal(Call("typeof", value), Str("Instance")), BinOp.And, MethodCall(value, "IsA", Str(native.Name)))
                : Equal(Call("typeof", value), Str(native.Name));
        }

        if (CollectionElement(element) is not null) {
            return NotEqual(LowerCollectionSafeCast(value, element), Nil());
        }

        if (IsTranspiledType(element) && element is INamedTypeSymbol named) {
            return NotEqual(LowerReferenceSafeCast(value, named, location), Nil());
        }

        return Bool(true);
    }

    private Expression LowerListCreation(HirObjectCreation creation) {
        var args = LowerArguments(creation.Arguments);
        var enumerable = creation.Arguments.Length == 1 && CollectionElement(creation.Arguments[0].Value.Type) is not null;
        if (!enumerable) {
            return NewCollection(creation.Type, Num(0), args.Count == 0 ? Num(0) : args[0], false, creation.Location);
        }

        var source = args[0] is SymbolExpression ? args[0] : CaptureValue(args[0], "source");
        var result = CaptureValue(NewCollection(creation.Type, Num(0), Index(source, "count"), false, creation.Location), "list");
        _names.Emit(Stmt(CollectionCall("AddRange", result, source)));

        return result;
    }

    private Expression LowerCollection(HirCollectionLiteral collection) {
        if (CollectionElement(collection.Type) is not { } element) {
            Unsupported($"Collection expressions targeting '{collection.Type.ToDisplayString()}' are not supported yet.", collection.Location);

            return Nil();
        }

        var isArray = collection.Type is IArrayTypeSymbol;
        var isReadOnly = !isArray && !IsList(collection.Type) && collection.Type.Name is not ("IList" or "ICollection");
        if ((isArray || isReadOnly) && collection.Length is null && collection.Elements.IsEmpty) {
            return Cast(CollectionCall("empty", RuntimeTypeKeyOf(element), Lambda([], Type("any"), [
                Return(NewCollection(collection.Type, Num(0), Num(0), true, collection.Location)),
            ])), LowerType(collection.Type, collection.Location));
        }

        var fixedLength = collection.Length is not null;
        var length = fixedLength ? CaptureValue(LowerExpression(collection.Length!), "length") : Num(0);
        var result = CaptureValue(NewCollection(collection.Type, length, length, false, collection.Location), "collection");

        for (var i = 0; i < collection.Elements.Length; i++) {
            if (collection.Elements[i] is HirSpread spread) {
                if (CollectionElement(spread.Operand.Type) is not { } sourceElement) {
                    Unsupported("Spreads currently require an array or a supported generic collection.", spread.Location);
                    continue;
                }

                var source = CaptureValue(LowerExpression(spread.Operand), "spread");
                var index = _names.FreshTemp("index");

                Expression value = CollectionCall("get", source, Name(index));
                value = ConvertCollectionElement(value, sourceElement, element, spread.Location);

                _names.Emit(Local(index, Num(0)));
                _names.Emit(CollectionWhile(Binary(Name(index), BinOp.LessThan, Index(source, "count")), [
                    Stmt(CollectionCall("push", result, value)),
                    Assign(VarName.FromString(index), Binary(Name(index), BinOp.Plus, Num(1))),
                ]));
            } else {
                var value = LowerExpression(collection.Elements[i]);

                _names.Emit(Stmt(fixedLength
                    ? CollectionCall("set", result, Num(i), value)
                    : CollectionCall("push", result, value)));
            }
        }

        if (isArray || isReadOnly) {
            _names.Emit(Stmt(CollectionCall("seal", result, Str(isArray ? "array" : "readonly"))));
        }

        if (!fixedLength) {
            _names.Emit(Assign(Index(result, "capacity"), Index(result, "count")));
        }

        return result;
    }

    private Expression ConvertCollectionElement(Expression value, ITypeSymbol source, ITypeSymbol target, Location location) {
        if (source.SpecialType == SpecialType.System_Char && target.SpecialType != SpecialType.System_Char && IsIntegral(target)) {
            return Call(Index("utf8", "codepoint"), value);
        }

        if (source.IsValueType && target.IsReferenceType) {
            if (source is INamedTypeSymbol { OriginalDefinition.SpecialType: SpecialType.System_Nullable_T } nullable) {
                return IfExpression.From(Equal(value, Nil()), Nil(), BoxValue(value, nullable.TypeArguments[0], location));
            }

            return BoxValue(value, source, location);
        }

        if (!source.IsValueType && target.IsValueType) {
            var nullable = target is INamedTypeSymbol { OriginalDefinition.SpecialType: SpecialType.System_Nullable_T };
            var underlying = nullable ? ((INamedTypeSymbol)target).TypeArguments[0] : target;

            return LowerBoxCast(value, underlying, false, nullable);
        }

        if (source.IsReferenceType && target.IsReferenceType && target.SpecialType != SpecialType.System_Object && !SymbolEqualityComparer.Default.Equals(source, target)) {
            var checkedValue = IfExpression.From(CollectionElementCheck(value, target, location), value, Nil());

            return CollectionCall("requireCast", checkedValue, value);
        }

        return value;
    }

    private Expression LowerCollectionIndex(HirIndexAccess index) {
        var (receiver, subscript) = LowerCollectionTarget(index, capture: false);

        return Cast(CollectionCall("get", receiver, subscript), LowerType(index.Type, index.Location));
    }

    private (Expression Receiver, Expression Index) LowerCollectionTarget(HirIndexAccess index, bool capture = true) {
        if (CollectionElement(index.Receiver.Type) is null || index.Indices.Length != 1) {
            Unsupported("Only single-index array and generic collection access is supported.", index.Location);

            return (Nil(), Num(0));
        }

        var receiver = LowerExpression(index.Receiver);
        var (subscript, preludes) = LowerCaptured(index.Indices[0]);
        if (capture || preludes.Count > 0) {
            receiver = CaptureValue(receiver, "collection");
        }

        foreach (var prelude in preludes) _names.Emit(prelude);

        return (receiver, capture ? CaptureValue(subscript, "index") : subscript);
    }

    private List<Statement> LowerCollectionWrite(HirIndexAccess index, HirExpression value, BinaryOperatorKind? op = null) {
        var (receiver, subscript) = LowerCollectionTarget(index);

        var previous = op is not null ? CaptureValue(CollectionCall("get", receiver, subscript), "element") : null;

        var concatenate = op == BinaryOperatorKind.Add && index.Type.SpecialType == SpecialType.System_String;
        var right = concatenate ? LowerDisplayValue(value) : LowerExpression(value);
        if (op is not null) {
            var operation = op.Value switch {
                BinaryOperatorKind.Add => index.Type.SpecialType == SpecialType.System_String ? BinOp.TwoDots : BinOp.Plus,
                BinaryOperatorKind.Subtract => BinOp.Minus,
                BinaryOperatorKind.Multiply => BinOp.Star,
                BinaryOperatorKind.Divide => BinOp.Slash,
                BinaryOperatorKind.Remainder => BinOp.Percent,

                _ => (BinOp?)null,
            };

            if (operation is null) {
                Unsupported($"Indexed compound operator '{op}' is not supported yet.", index.Location);

                return [];
            }

            if (concatenate) {
                previous = Call("tostring", IfExpression.From(Equal(previous!, Nil()), Str(""), previous!));
                right = Call("tostring", right);
            }

            right = op == BinaryOperatorKind.Remainder
                ? Call(Index("math", "fmod"), previous!, right)
                : Binary(previous!, operation.Value, right);

            if (op == BinaryOperatorKind.Divide && IsIntegral(index.Type)) {
                right = Call(Index("math", "modf"), right);
            }
        }

        return [Stmt(CollectionCall("set", receiver, subscript, right))];
    }

    private List<Statement> LowerCapacityCompound(HirPropertyAccess access, BinaryOperatorKind op, HirExpression value) {
        return WithStableReceiver(access, stable => {
            var property = (HirPropertyAccess)stable;
            var combined = new HirBinary {
                Location = access.Location,
                Type = access.Type,
                Left = property,
                Right = value,
                Op = op,
                IsChecked = false,
                OperatorMethod = null,
            };

            return [Stmt(CollectionCall("capacity", LowerExpression(property.Receiver!), LowerExpression(combined)))];
        });
    }

    private Expression UnsupportedCollectionProperty(HirPropertyAccess access) {
        Unsupported($"Collection property '{access.Symbol.Name}' is not supported yet.", access.Location);

        return Nil();
    }

    private bool TryLowerCollectionCall(HirCall call, Expression? receiver, List<Expression> args, out FunctionCallExpression? result) {
        result = null;
        var method = call.Method;

        if (receiver is not null && CollectionElement(call.Receiver?.Type ?? method.ContainingType) is not null) {
            switch (method.Name) {
                case "Sort": {
                    var comparison = args.Count == 1 && method.Parameters[0].Type.TypeKind == TypeKind.Delegate ? args[0] : Nil();
                    if (args.Count > 0 && comparison is SymbolExpression && method.Parameters[^1].Type.TypeKind != TypeKind.Delegate) {
                        Unsupported("Sort currently supports the default ordering or a Comparison<T> lambda.", call.Location);

                        return true;
                    }

                    result = CollectionCall("Sort", receiver, Num(0), Index(receiver, "count"), comparison);

                    return true;
                }

                case "FindIndex":
                case "FindLastIndex": {
                    var start = args.Count == 1 ? method.Name == "FindIndex" ? Num(0) : Binary(Index(receiver, "count"), BinOp.Minus, Num(1)) : args[0];
                    var count = args.Count == 3 ? args[1] : method.Name == "FindIndex" ? Binary(Index(receiver, "count"), BinOp.Minus, start) : Binary(start, BinOp.Plus, Num(1));

                    result = CollectionCall(method.Name, receiver, start, count, args[^1]);

                    return true;
                }

                case "ConvertAll": {
                    var destination = NewCollection(method.ReturnType, Num(0), Index(receiver, "count"), false, call.Location);
                    result = CollectionCall("ConvertAll", receiver, args[0], destination);

                    return true;
                }

                case "ForEach":
                case "Find":
                case "FindLast":
                case "FindAll":
                case "Exists":
                case "TrueForAll":
                case "RemoveAll": {
                    result = CollectionCall(method.Name, [receiver, ..args]);

                    return true;
                }

                case "CopyTo": {
                    result = args.Count == 4
                        ? CollectionCall("CopyTo", receiver, args[0], args[1], args[2], args[3])
                        : CollectionCall("CopyTo", receiver, Num(0), args[0], args.Count == 2 ? args[1] : Num(0), Index(receiver, "count"));

                    return true;
                }

                case "ToString":
                case "Add":
                case "AddRange":
                case "Insert":
                case "InsertRange":
                case "Remove":
                case "RemoveAt":
                case "RemoveRange":
                case "Clear":
                case "Contains":
                case "IndexOf":
                case "LastIndexOf":
                case "Reverse":
                case "ToArray":
                case "GetRange":
                case "EnsureCapacity":
                case "TrimExcess":
                case "GetLength":
                case "GetLowerBound":
                case "GetUpperBound":
                case "Clone": {
                    result = CollectionCall(method.Name, [receiver, ..args]);

                    return true;
                }
            }

            Unsupported($"Collection method '{method.ToDisplayString()}' is not supported yet.", call.Location);

            return true;
        }

        if (method.ContainingType.SpecialType == SpecialType.System_Array && method.Name != "Empty") {
            switch (method.Name) {
                case "Reverse":
                case "IndexOf":
                case "LastIndexOf": {
                    result = CollectionCall(method.Name, args);

                    return true;
                }

                case "Sort" when args.Count == 1: {
                    result = CollectionCall("Sort", args[0], Num(0), Index(args[0], "count"), Nil());

                    return true;
                }

                case "Clear": {
                    result = CollectionCall("ArrayClear", args[0], args.Count == 1 ? Num(0) : args[1], args.Count == 1 ? Index(args[0], "count") : args[2]);

                    return true;
                }

                case "Copy" when args.Count is 3 or 5: {
                    var ordered = call.Arguments.OrderBy(argument => argument.Symbol.Ordinal).ToArray();
                    var source = ArrayArgumentElement(ordered[0].Value);
                    var destination = ArrayArgumentElement(ordered[args.Count == 3 ? 1 : 2].Value);

                    if (source is null || destination is null ||
                        !SymbolEqualityComparer.Default.Equals(source, destination) && (source.IsValueType || destination.IsValueType)) {
                        Unsupported("Array.Copy currently requires matching element types or two reference-type arrays.", call.Location);

                        return true;
                    }

                    result = args.Count == 3
                        ? CollectionCall("CopyTo", args[0], Num(0), args[1], Num(0), args[2])
                        : CollectionCall("CopyTo", args[0], args[1], args[2], args[3], args[4]);

                    return true;
                }
            }
        }

        if (method.ContainingType.SpecialType == SpecialType.System_Array && method.Name == "Empty") {
            var type = (IArrayTypeSymbol)method.ReturnType;
            result = CollectionCall("empty", RuntimeTypeKeyOf(type.ElementType),
                Lambda([], Type("any"), [Return(NewCollection(type, Num(0), Num(0), true, call.Location))])
            );

            return true;
        }

        return false;
    }

    private static ITypeSymbol? ArrayArgumentElement(HirExpression expression) {
        while (expression is HirConversion conversion) expression = conversion.Operand;

        return expression.Type is IArrayTypeSymbol { Rank: 1 } array ? array.ElementType : null;
    }

    private static BinaryOperatorExpression Binary(Expression left, BinOp op, Expression right) => new() { Left = left, Op = op, Right = right };
    private static WhileStatement CollectionWhile(Expression condition, List<Statement> body) => new() { Condition = condition, Block = BlockOf(body) };
    private static IfStatement CollectionIf(Expression condition, params IEnumerable<Statement> body) => new() { Condition = condition, Block = BlockOf([..body]) };
}