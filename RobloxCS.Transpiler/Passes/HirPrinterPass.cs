using Microsoft.CodeAnalysis;
using RobloxCS.HIR;
using RobloxCS.HIR.Declarations;
using RobloxCS.HIR.Expressions;
using RobloxCS.HIR.Statements;
using Spectre.Console;

namespace RobloxCS.Transpiler.Passes;

public sealed class HirPrinterPass : IPass {
    public string Name => "HIR Printer";
    public List<string> Diagnostics { get; } = [];

    public HirModule Run(HirModule module, TranspilationContext ctx) {
        foreach (var type in module.Types) {
            AnsiConsole.MarkupLine($"[cyan]class[/] [white]{type.Symbol.Name}[/]");

            foreach (var field in type.Fields) {
                FormatField(field, 1);
            }

            foreach (var method in type.Methods) {
                FormatMethod(method, 1);
            }
        }

        return module;
    }

    private static void FormatMethod(HirMethod method, int depth) {
        var padding = FormatDepth(depth);
        var methodDisplay = method.IsStatic ? "static method" : "method";
        var isImplicitCtor = method is { IsConstructor: true, IsImplicit: true } ? "implicit " : string.Empty;
        var methodPrefix = method.IsConstructor ? $" [lime]{isImplicitCtor}constructor[/]" : string.Empty;
        var isEntryPoint = method.IsEntryPoint ? " [lime]entry point[/]" : string.Empty;

        AnsiConsole.MarkupLine($"{padding}[cyan]{methodDisplay}[/] [white]{method.Symbol.Name}[/]{methodPrefix}{isEntryPoint}");

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

        switch (statement) {
            case HirLocalDeclaration localDeclaration: {
                foreach (var decl in localDeclaration.Declarators) {
                    AnsiConsole.MarkupLine($"{padding}[cyan]local declaration[/] [white]{decl.Symbol.Name}[/]");

                    if (decl.Initializer is not null) {
                        FormatExpression(decl.Initializer, depth + 1);
                    }
                }

                break;
            }

            case HirAssignment assignment: {
                AnsiConsole.MarkupLine($"{padding}[cyan]assignment[/]");
                FormatExpression(assignment.Target, depth + 1);
                FormatExpression(assignment.Value, depth + 1);

                break;
            }

            case HirExpressionStatement expressionStatement: {
                AnsiConsole.MarkupLine($"{padding}[cyan]expression statement[/]");
                FormatExpression(expressionStatement.Expression, depth + 1);

                break;
            }

            default: {
                AnsiConsole.MarkupLineInterpolated($"{padding}[red]unhandled statement[/] [white]{statement.GetType().Name}[/]");

                break;
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

    private static void FormatExpression(HirExpression expression, int depth) {
        var padding = FormatDepth(depth);

        switch (expression) {
            case HirLiteral literal: {
                AnsiConsole.MarkupLine($"{padding}[yellow]{literal.Type} literal[/] [white]{literal.Value!}[/]");

                break;
            }

            case HirLocalRef localRef: {
                AnsiConsole.MarkupLine($"{padding}[yellow]local ref[/] [white]{localRef.Symbol.Name}[/]");

                break;
            }

            case HirCall call: {
                var extensionPrefix = call.IsExtension ? " [lime]extension[/]" : string.Empty;
                var staticPrefix = call.Method.IsStatic ? " [lime]static[/]" : string.Empty;
                var containingPrefix = call.Method.IsStatic ? $" [yellow]{call.Method.ContainingSymbol}[/]" : string.Empty;

                AnsiConsole.MarkupLine($"{padding}[cyan]call[/] [white]{call.Method.Name}[/]{staticPrefix}{extensionPrefix}{containingPrefix}");

                foreach (var arg in call.Arguments) {
                    FormatExpression(arg, depth + 1);
                }

                break;
            }

            case HirArgument argument: {
                AnsiConsole.MarkupLine($"{padding}[cyan]argument[/]");
                FormatExpression(argument.Value, depth + 1);

                break;
            }

            case HirParameterRef paramRef: {
                AnsiConsole.MarkupLine($"{padding}[yellow]param ref[/] [white]{paramRef.Symbol.Name}[/]");
                
                break;
            }

            case HirFieldAccess fieldAccess: {
                var target = fieldAccess.Symbol.Name;

                AnsiConsole.MarkupLine($"{padding}[cyan]field access[/] [white]{target}[/]");
                if (fieldAccess.Receiver is not null) {
                    FormatExpression(fieldAccess.Receiver, depth + 1);
                }

                break;
            }

            case HirThis @this: {
                AnsiConsole.MarkupLine($"{padding}[magenta]this[/]");

                break;
            }

            default: {
                AnsiConsole.MarkupLine($"{padding}[red]unhandled expression[/] [white]{expression.GetType().Name}[/]");

                break;
            }
        }
    }

    private static string FormatDepth(int depth) => string.Concat(Enumerable.Repeat("  ", depth));
}