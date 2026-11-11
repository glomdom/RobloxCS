using RobloxCS.AST.Expressions;
using RobloxCS.AST.Statements;
using RobloxCS.HIR;
using RobloxCS.HIR.Declarations;
using Serilog;
using static RobloxCS.Lowering.Luau;

namespace RobloxCS.Lowering;

public sealed partial class HirLowerer {
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

    private static Expression NewInstance(string name, bool hasFields) {
        Expression seed = hasFields ? Cast(Table(), Type(FieldsTypeName(name))) : Table();

        return Call("setmetatable", seed, Name(name));
    }

    private static string FieldsTypeName(string className) => $"{className}Fields";
}
