using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.CodeAnalysis.Operations;
using RobloxCS.HIR.Declarations;
using RobloxCS.HIR.Expressions;
using Serilog;

namespace RobloxCS.HIR.Builder;

public sealed partial class HirBuilder {
    public HirField BuildField(IFieldSymbol field) {
        if (field.CapturedPrimaryParameter() is { } parameter) {
            return new HirField {
                Location = parameter.Locations.ResolveSingle(),
                Symbol = field,
                Name = field.SlotName(),
                IsStatic = false,
                Initializer = new HirParameterRef {
                    Location = parameter.Locations.ResolveSingle(),
                    Type = parameter.Type,
                    Symbol = parameter,
                },
            };
        }

        if (field.IsImplicitlyDeclared) {
            Log.Verbose("Skipping {FieldName} as it is implicitly declared", field.Name);
        }

        var syntax = Semantics.GetFirstSyntaxFromSymbol<VariableDeclaratorSyntax>(field);

        HirExpression? initializer = null;
        if (syntax.Initializer is not null) {
            var operation = Semantics.CheckedGetOperation<IFieldInitializerOperation>(syntax.Initializer);

            initializer = BuildExpression(operation.Value);
        }

        return new HirField {
            Location = field.Locations.ResolveSingle(),
            Symbol = field,
            Initializer = initializer,
            Name = field.Name,
            IsStatic = field.IsStatic,
        };
    }
}
