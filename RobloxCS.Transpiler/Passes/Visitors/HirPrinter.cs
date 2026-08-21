using Microsoft.CodeAnalysis;
using RobloxCS.HIR;
using RobloxCS.HIR.Declarations;
using RobloxCS.HIR.Expressions;
using RobloxCS.HIR.Statements;
using Spectre.Console;

namespace RobloxCS.Transpiler.Passes.Visitors;

public sealed class HirPrinter : HirVisitor {
    private int _depth;

    protected override void VisitType(HirType node) {
        Line($"[cyan]class[/] [white]{node.Symbol.Name}[/]");

        _depth++;
        base.VisitType(node);
        _depth--;
    }

    protected override void VisitField(HirField node) {
        Line($"[cyan]field[/] [white]{node.Symbol.Name}[/]");

        _depth++;
        base.VisitField(node);
        _depth--;
    }

    protected override void VisitMethod(HirMethod node) {
        var methodDisplay = node.IsStatic ? "static method" : "method";
        var isImplicitCtor = node is { IsConstructor: true, IsImplicit: true } ? "implicit " : string.Empty;
        var methodPrefix = node.IsConstructor ? $" [lime]{isImplicitCtor}constructor[/]" : string.Empty;
        var isEntryPoint = node.IsEntryPoint ? " [lime]entry point[/]" : string.Empty;

        Line($"[cyan]{methodDisplay}[/] [white]{node.Symbol.Name}[/]{methodPrefix}{isEntryPoint}");

        _depth++;
        base.VisitMethod(node);
        if (node.Block is null) Line("[magenta]no block[/]");
        _depth--;
    }

    protected override void VisitParameter(HirParameter node) {
        var defaultParameterDisplay = node.DefaultValue is not null ? " [lime]with default value[/]" : null;

        Line($"[yellow]{Escape(node.Symbol.Type)} parameter[/] [white]{node.Symbol.Name}[/]{defaultParameterDisplay}");

        _depth++;
        base.VisitParameter(node);
        _depth--;
    }

    protected override void VisitBlock(HirBlock node) {
        Line("[cyan]block[/]");

        _depth++;

        Line("[cyan]local definitions[/]");
        _depth++;
        foreach (var local in node.Locals) PrintLocal(local);
        _depth--;

        Line("[cyan]statements[/]");
        _depth++;
        base.VisitBlock(node);
        _depth--;

        _depth--;
    }

    protected override void VisitVariableDeclarator(HirVariableDeclarator node) {
        Line($"[cyan]local declaration[/] [white]{node.Symbol.Name}[/]");

        _depth++;
        base.VisitVariableDeclarator(node);
        _depth--;
    }

    protected override void VisitAssignment(HirAssignment node) {
        Line("[cyan]assignment[/]");

        _depth++;
        base.VisitAssignment(node);
        _depth--;
    }

    protected override void VisitExpressionStatement(HirExpressionStatement node) {
        Line("[cyan]expression statement[/]");

        _depth++;
        base.VisitExpressionStatement(node);
        _depth--;
    }

    protected override void VisitArgument(HirArgument node) {
        Line("[cyan]argument[/]");

        _depth++;
        base.VisitArgument(node);
        _depth--;
    }

    protected override void VisitCall(HirCall node) {
        var extensionPrefix = node.IsExtension ? " [lime]extension[/]" : string.Empty;
        var staticPrefix = node.Method.IsStatic ? " [lime]static[/]" : string.Empty;
        var containingPrefix = node.Method.IsStatic ? $" [yellow]{Escape(node.Method.ContainingSymbol)}[/]" : string.Empty;

        Line($"[cyan]call[/] [white]{node.Method.Name}[/]{staticPrefix}{extensionPrefix}{containingPrefix}");

        _depth++;
        base.VisitCall(node);
        _depth--;
    }

    protected override void VisitFieldAccess(HirFieldAccess node) {
        Line($"[cyan]field access[/] [white]{node.Symbol.Name}[/]");

        _depth++;
        base.VisitFieldAccess(node);
        _depth--;
    }

    protected override void VisitLiteral(HirLiteral node) {
        var type = Escape(node.Type);
        var value = Escape(node.Value);

        Line($"[yellow]{type} literal[/] [white]{value}[/]");
    }

    protected override void VisitLocalRef(HirLocalRef node) {
        Line($"[yellow]local ref[/] [white]{node.Symbol.Name}[/]");
    }

    protected override void VisitParameterRef(HirParameterRef node) {
        Line($"[yellow]param ref[/] [white]{node.Symbol.Name}[/]");
    }

    protected override void VisitThis(HirThis node) {
        Line("[magenta]this[/]");
    }

    public override void VisitStatement(HirStatement node) {
        switch (node) {
            case HirAssignment:
            case HirBlock:
            case HirExpressionStatement:
            case HirLocalDeclaration:
                base.VisitStatement(node);
                break;

            default:
                Line($"[red]unhandled statement[/] [white]{node.GetType().Name}[/]");

                break;
        }
    }

    public override void VisitExpression(HirExpression node) {
        switch (node) {
            case HirCall:
            case HirFieldAccess:
            case HirLiteral:
            case HirLocalRef:
            case HirParameterRef:
            case HirThis: {
                base.VisitExpression(node);

                break;
            }

            default: {
                Line($"[red]unhandled expression[/] [white]{node.GetType().Name}[/]");

                break;
            }
        }
    }

    private void PrintLocal(ILocalSymbol symbol) {
        Line($"[yellow]local {Escape(symbol.Type)}[/] [white]{Escape(symbol)}[/]");
    }

    private void Line(string markup) {
        AnsiConsole.MarkupLine($"{Padding()}{markup}");
    }

    private string Padding() => string.Concat(Enumerable.Repeat("  ", _depth));

    private static string Escape(object? value) => Markup.Escape(value?.ToString() ?? "null");
}