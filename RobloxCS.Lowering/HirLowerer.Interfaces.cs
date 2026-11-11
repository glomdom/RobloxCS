using Microsoft.CodeAnalysis;
using RobloxCS.AST.Expressions;
using RobloxCS.AST.Statements;
using RobloxCS.AST.Types;
using RobloxCS.HIR;
using RobloxCS.HIR.Declarations;
using RobloxCS.HIR.Expressions;
using static RobloxCS.Lowering.Luau;
using TypeInfo = RobloxCS.AST.Types.TypeInfo;

namespace RobloxCS.Lowering;

public sealed partial class HirLowerer {
    private static IEnumerable<IMethodSymbol> InterfaceMethods(INamedTypeSymbol contract) => contract.GetMembers().OfType<IMethodSymbol>()
        .Where(method => !method.IsStatic && method.MethodKind is MethodKind.Ordinary or MethodKind.PropertyGet or MethodKind.PropertySet);

    private TypeInfo LowerMethodType(IMethodSymbol method) => new CallbackTypeInfo {
        Arguments = [
            TypeArgument.From("self", Type("any")),
            ..method.Parameters.Select(parameter => TypeArgument.From(parameter.Name, LowerType(parameter.Type, parameter.Locations.FirstOrDefault() ?? Location.None))),
        ],
        ReturnType = LowerType(method.ReturnType, method.Locations.FirstOrDefault() ?? Location.None),
    };

    private List<Statement> LowerInterface(HirType type) {
        var symbol = type.Symbol;
        if (symbol.IsGenericType || symbol.ContainingType is not null) {
            Unsupported("Generic and nested interfaces are not supported yet.", type.Location);

            return [];
        }

        var fields = new Dictionary<string, TypeInfo>(StringComparer.Ordinal);
        foreach (var contract in symbol.AllInterfaces.Prepend(symbol)) {
            if (!IsTranspiledType(contract)) {
                Unsupported("Interfaces from external assemblies are not supported yet.", type.Location);

                continue;
            }

            foreach (var member in contract.GetMembers()) {
                if (member.IsStatic || member is IEventSymbol or IPropertySymbol { IsIndexer: true }) {
                    Unsupported("Static interface members, events, and indexers are not supported yet.", member.Locations.FirstOrDefault() ?? type.Location);
                }
            }

            foreach (var method in InterfaceMethods(contract)) {
                if (
                    !method.IsAbstract || method.IsGenericMethod || method.Parameters.Any(parameter => parameter.RefKind != RefKind.None || parameter.IsParams) ||
                    method.ReturnsByRef || method.ReturnsByRefReadonly
                ) {
                    Unsupported(
                        "Default interface implementations, generic methods, and ref/params interface members are not supported yet.",
                        method.Locations.FirstOrDefault() ?? type.Location
                    );

                    continue;
                }

                fields.TryAdd(method.SlotName(), LowerMethodType(method));
            }
        }

        return [TypeDecl(TypeName(symbol), TableType(fields.Select(field => (field.Key, field.Value))), true)];
    }

    private List<Statement> LowerInterfaceBindings(HirType type) {
        var statements = new List<Statement>();
        foreach (var contract in type.Symbol.AllInterfaces) {
            if (!IsTranspiledType(contract)) {
                Unsupported("Implementing generic or external interfaces is not supported yet.", type.Location);

                continue;
            }

            foreach (var member in InterfaceMethods(contract)) {
                if (type.Symbol.FindImplementationForInterfaceMember(member) is not IMethodSymbol implementation) {
                    continue;
                }

                var autoProperty = implementation.AssociatedSymbol is IPropertySymbol property && property.IsAutoProperty();
                if (!autoProperty && implementation.SlotName() == member.SlotName()) {
                    continue;
                }

                _names.BeginFunction();

                var parameters = new List<(string Name, TypeInfo Type)> { ("self", Type("any")) };
                var arguments = new List<Expression>();
                foreach (var parameter in member.Parameters) {
                    var name = _names.NameFor(parameter);

                    parameters.Add((name, LowerType(parameter.Type, parameter.Locations.FirstOrDefault() ?? type.Location)));
                    arguments.Add(Name(name));
                }

                List<Statement> body;

                if (autoProperty && !implementation.IsVirtual && !implementation.IsOverride) {
                    var backing = ((IPropertySymbol)implementation.AssociatedSymbol!).BackingField()!;
                    body = member.MethodKind == MethodKind.PropertyGet
                        ? [Return(Index("self", backing.SlotName()))]
                        : [Assign(Index("self", backing.SlotName()), arguments[0])];
                } else {
                    var call = implementation.IsVirtual || implementation.IsOverride || implementation.IsAbstract
                        ? MethodCall(Name("self"), implementation.SlotName(), arguments)
                        : Call(Index(ReferenceType(implementation.ContainingType, type.Location), implementation.SlotName()), [Name("self"), ..arguments]);
                    body = implementation.ReturnsVoid ? [Stmt(call)] : [Return(call)];
                }

                statements.Add(Function(TypeName(type.Symbol), member.SlotName(), parameters, LowerType(member.ReturnType, type.Location), body));
            }
        }

        return statements;
    }
}