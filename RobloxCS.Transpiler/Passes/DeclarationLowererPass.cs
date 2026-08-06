using Microsoft.CodeAnalysis;
using RobloxCS.HIR;
using RobloxCS.HIR.Declarations;
using RobloxCS.HIR.Expressions;
using RobloxCS.HIR.Statements;
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
            AnsiConsole.MarkupLine($"[cyan]class[/] [white]{type.Symbol.Name}[/]");

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
        var methodPrefix = method.IsConstructor ? " [lime]constructor[/]" : null;

        AnsiConsole.MarkupLine($"{padding}[cyan]{methodDisplay}[/] [white]{method.Symbol.Name}[/]{methodPrefix}");

        foreach (var param in method.Parameters) {
            FormatParameter(param, depth + 1);
        }

        if (method.Block is { } block) {
            FormatBlock(block, depth + 1);
        } else {
            AnsiConsole.MarkupLine($"{FormatDepth(depth + 1)}[magenta]no block[/]");
        }
    }

    private static void FormatBlock(HirBlock block, int depth) {
        var padding = FormatDepth(depth);

        AnsiConsole.MarkupLine($"{padding}[cyan]block[/]");
        AnsiConsole.MarkupLine($"{FormatDepth(depth + 1)}[cyan]local definitions[/]");

        foreach (var local in block.Locals) {
            FormatLocal(local, depth + 2);
        }

        AnsiConsole.MarkupLine($"{FormatDepth(depth + 1)}[cyan]statements[/]");

        foreach (var stmt in block.Statements) {
            FormatStatement(stmt, depth + 2);
        }
    }

    private static void FormatStatement(HirStatement statement, int depth) {
        var padding = FormatDepth(depth);

        if (statement is HirLocalDeclaration localDeclaration) {
            foreach (var decl in localDeclaration.Declarators) {
                AnsiConsole.MarkupLine($"{padding}[cyan]local declaration[/] [white]{decl.Symbol.Name}[/]");

                if (decl.Initializer is not null) {
                    FormatExpression(decl.Initializer, depth + 1);
                }
            }
        }
    }

    private static void FormatLocal(ILocalSymbol symbol, int depth) {
        var padding = FormatDepth(depth);

        AnsiConsole.MarkupLine($"{padding}[yellow]local {symbol.Type}[/] [white]{symbol}[/]");
    }

    private static void FormatParameter(HirParameter parameter, int depth) {
        var padding = FormatDepth(depth);

        var defaultParameterDisplay = parameter.DefaultValue is not null ? " [lime]with default value[/]" : null;
        AnsiConsole.MarkupLine($"{padding}[yellow]{parameter.Symbol.Type} parameter[/] [white]{parameter.Symbol.Name}[/]{defaultParameterDisplay}");

        if (parameter.DefaultValue is { } defaultValue) {
            FormatExpression(defaultValue, depth + 1);
        }
    }

    private static void FormatField(HirField field, int depth) {
        var padding = FormatDepth(depth);

        AnsiConsole.MarkupLine($"{padding}[cyan]field[/] [white]{field.Symbol.Name}[/]");

        if (field.Initializer is { } initializer) {
            FormatExpression(initializer, depth + 1);
        }
    }

    private static void FormatExpression(HirExpression initializer, int depth) {
        var padding = FormatDepth(depth);

        var format = string.Empty;
        if (initializer is HirLiteral literal) {
            format = $"[yellow]{literal.Type} literal[/] [white]{literal.Value!}[/]";
        }

        AnsiConsole.MarkupLine($"{padding}{format}");
    }

    private static string FormatDepth(int depth) => string.Concat(Enumerable.Repeat("  ", depth));
}