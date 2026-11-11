using System.Collections.Immutable;
using Microsoft.CodeAnalysis;
using RobloxCS.AST.Statements;
using RobloxCS.HIR;
using RobloxCS.HIR.Declarations;
using Serilog;
using static RobloxCS.Lowering.Luau;
using TypeInfo = RobloxCS.AST.Types.TypeInfo;

namespace RobloxCS.Lowering;

public sealed partial class HirLowerer {
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
}
