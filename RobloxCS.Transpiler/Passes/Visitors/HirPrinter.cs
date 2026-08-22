using Microsoft.CodeAnalysis;
using RobloxCS.HIR;
using RobloxCS.HIR.Declarations;
using RobloxCS.HIR.Expressions;
using RobloxCS.HIR.Statements;
using Spectre.Console;

namespace RobloxCS.Transpiler.Passes.Visitors;

public sealed class HirPrinter : HirVisitor {
    private const string Guide = "[grey]··[/]";

    private int _depth;

    public override void VisitModule(HirModule node) {
        Line($"[cyan]module[/] [white]{Escape(node.SourcePath)}[/]");

        _depth++;
        base.VisitModule(node);
        _depth--;
    }

    protected override void VisitType(HirType node) {
        var baseType = node.Base is not null ? $" [grey]: {Type(node.Base)}[/]" : string.Empty;

        Line($"[cyan]class[/] [white]{node.Symbol.Name}[/]{baseType}");

        _depth++;
        base.VisitType(node);
        _depth--;
    }

    protected override void VisitField(HirField node) {
        Line($"[cyan]field[/] [white]{node.Symbol.Name}[/] [grey]{Type(node.Symbol.Type)}[/]{Flags(node.IsStatic ? "static" : null)}");

        _depth++;
        base.VisitField(node);
        _depth--;
    }

    protected override void VisitMethod(HirMethod node) {
        var flags = Flags(
            node.IsStatic ? "static" : null,
            node.IsConstructor ? "constructor" : null,
            node.IsImplicit ? "implicit" : null,
            node.IsEntryPoint ? "entry point" : null
        );

        Line($"[cyan]method[/] [white]{node.Symbol.Name}[/] [grey]-> {Type(node.Symbol.ReturnType)}[/]{flags}");

        _depth++;
        base.VisitMethod(node);
        if (node.Block is null) Line("[grey italic]no block[/]");
        _depth--;
    }

    protected override void VisitProperty(HirProperty node) {
        var flags = Flags(
            node.IsStatic ? "static" : null,
            node.IsAuto ? "auto" : null
        );

        Line($"[cyan]property[/] [white]{node.Symbol.Name}[/] [grey]{Type(node.Symbol.Type)}[/]{flags}");

        _depth++;
        base.VisitProperty(node);
        _depth--;
    }

    protected override void VisitParameter(HirParameter node) {
        var flags = Flags(
            node.IsParams ? "params" : null,
            node.RefKind is not RefKind.None ? node.RefKind.ToString().ToLowerInvariant() : null
        );

        Line($"[cyan]parameter[/] [white]{node.Symbol.Name}[/] [grey]{Type(node.Symbol.Type)}[/]{flags}");

        _depth++;
        base.VisitParameter(node);
        _depth--;
    }

    protected override void VisitBlock(HirBlock node) {
        Line("[cyan]block[/]");

        _depth++;

        if (node.Locals.Length > 0) {
            Line("[grey]locals[/]");

            _depth++;
            foreach (var local in node.Locals) {
                Line($"[white]{local.Name}[/] [grey]{Type(local.Type)}[/]");
            }

            _depth--;
        }

        base.VisitBlock(node);

        _depth--;
    }

    protected override void VisitVariableDeclarator(HirVariableDeclarator node) {
        var uninitialized = node.Initializer is null ? " [grey italic]uninitialized[/]" : string.Empty;

        Line($"[cyan]declare[/] [white]{node.Symbol.Name}[/] [grey]{Type(node.Symbol.Type)}[/]{uninitialized}");

        _depth++;
        base.VisitVariableDeclarator(node);
        _depth--;
    }

    protected override void VisitAssignment(HirAssignment node) {
        Line("[cyan]assign[/]");

        _depth++;
        base.VisitAssignment(node);
        _depth--;
    }

    protected override void VisitExpressionStatement(HirExpressionStatement node) {
        base.VisitExpressionStatement(node);
    }

    protected override void VisitArgument(HirArgument node) {
        Line($"[grey]arg[/] [white]{node.Symbol.Name}[/][grey]:[/]");

        _depth++;
        base.VisitArgument(node);
        _depth--;
    }

    protected override void VisitCall(HirCall node) {
        var target = node.Method.IsStatic
            ? $"[grey]{Type(node.Method.ContainingType)}.[/][white]{node.Method.Name}[/]"
            : $"[white]{node.Method.Name}[/]";

        var flags = Flags(
            node.Method.IsStatic ? "static" : null,
            node.IsExtension ? "extension" : null
        );

        Line($"[cyan]call[/] {target} [grey]-> {Type(node.Method.ReturnType)}[/]{flags}");

        _depth++;
        base.VisitCall(node);
        _depth--;
    }

    protected override void VisitFieldAccess(HirFieldAccess node) {
        var target = node.Symbol.IsStatic
            ? $"[grey]{Type(node.Symbol.ContainingType)}.[/][white]{node.Symbol.Name}[/]"
            : $"[white]{node.Symbol.Name}[/]";

        Line($"[cyan]field[/] {target}");

        _depth++;
        base.VisitFieldAccess(node);
        _depth--;
    }

    protected override void VisitLiteral(HirLiteral node) {
        Line($"[cyan]literal[/] {LiteralValue(node.Value)} [grey]{Type(node.Type)}[/]");
    }

    protected override void VisitLocalRef(HirLocalRef node) {
        Line($"[cyan]local[/] [white]{node.Symbol.Name}[/]");
    }

    protected override void VisitParameterRef(HirParameterRef node) {
        Line($"[cyan]param[/] [white]{node.Symbol.Name}[/]");
    }

    protected override void VisitThis(HirThis node) {
        Line("[magenta]this[/]");
    }

    public override void VisitStatement(HirStatement node) {
        switch (node) {
            case HirAssignment:
            case HirBlock:
            case HirExpressionStatement:
            case HirLocalDeclaration: {
                base.VisitStatement(node);

                break;
            }

            default: {
                Line($"[red]unhandled statement[/] [white]{node.GetType().Name}[/]");

                break;
            }
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

    private void Line(string markup) {
        AnsiConsole.MarkupLine($"{Padding()}{markup}");
    }

    private string Padding() => string.Concat(Enumerable.Repeat(Guide, _depth));

    private static string Flags(params string?[] flags) {
        var present = flags.Where(flag => flag is not null).ToList();
        if (present.Count == 0) return string.Empty;

        return $" [grey italic]{string.Join(" ", present)}[/]";
    }

    private static string LiteralValue(object? value) => value switch {
        null => "[magenta]null[/]",
        bool b => $"[magenta]{(b ? "true" : "false")}[/]",
        string s => $"[lime]\"{Escape(s)}\"[/]",
        char c => $"[lime]'{Escape(c)}'[/]",

        _ => $"[yellow]{Escape(value)}[/]",
    };

    private static string Type(ISymbol? symbol) =>
        Escape(symbol?.ToDisplayString(SymbolDisplayFormat.MinimallyQualifiedFormat));

    private static string Escape(object? value) => Markup.Escape(value?.ToString() ?? "?");
}