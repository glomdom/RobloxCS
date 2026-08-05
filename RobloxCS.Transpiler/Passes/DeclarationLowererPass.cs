using RobloxCS.HIR;
using RobloxCS.HIR.Declarations;
using RobloxCS.HIR.Expressions;
using RobloxCS.Transpiler.Builders.HIR;
using Spectre.Console;

namespace RobloxCS.Transpiler.Passes;

public sealed class DeclarationLowererPass : IPass {
    public string Name => "Declaration Lowerer";
    public List<string> Diagnostics { get; } = [];

    public void Run(TranspilationContext ctx) {
        var builder = new HirBuilder(ctx);
        var module = builder.Build();

        foreach (var type in module.Types) {
            AnsiConsole.WriteLine($"{type.Symbol.Name}");

            foreach (var field in type.Fields) {
                FormatField(field, 1);
            }

            foreach (var method in type.Methods) {
                FormatMethod(method, 1);
            }
        }
    }

    private static void FormatMethod(HirMethod method, int depth) {
        var padding = FormatDepth(depth);
        var methodDisplay = method.IsStatic ? "static method" : "method";

        AnsiConsole.MarkupLine($"{padding}[cyan]{methodDisplay}[/] {method.Symbol.Name}");

        foreach (var param in method.Parameters) {
            FormatParameter(param, depth + 1);
        }
    }

    private static void FormatParameter(HirParameter parameter, int depth) {
        var padding = FormatDepth(depth);

        var defaultParameterDisplay = parameter.DefaultValue is not null ? " [lime]with default value[/]" : null;
        AnsiConsole.MarkupLine($"{padding}[yellow]{parameter.Symbol.Type} parameter[/] {parameter.Symbol.Name}{defaultParameterDisplay}");

        if (parameter.DefaultValue is { } defaultValue) {
            FormatExpression(defaultValue, depth + 1);
        }
    }

    private static void FormatField(HirField field, int depth) {
        var padding = FormatDepth(depth);

        AnsiConsole.MarkupLine($"{padding}[cyan]field[/] {field.Symbol.Name}");

        if (field.Initializer is { } initializer) {
            FormatExpression(initializer, depth + 1);
        }
    }

    private static void FormatExpression(HirExpression initializer, int depth) {
        var padding = FormatDepth(depth);

        var format = string.Empty;
        if (initializer is HirLiteral literal) {
            format = $"[yellow]{literal.Type} literal[/] {literal.Value!}";
        }

        AnsiConsole.MarkupLine($"{padding}{format}");
    }

    private static string FormatDepth(int depth) => string.Concat(Enumerable.Repeat("  ", depth));
}