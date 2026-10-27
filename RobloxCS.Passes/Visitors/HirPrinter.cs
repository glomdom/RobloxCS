using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.Operations;
using RobloxCS.HIR;
using RobloxCS.HIR.Declarations;
using RobloxCS.HIR.Expressions;
using RobloxCS.HIR.Interpolation;
using RobloxCS.HIR.Statements;
using Spectre.Console;

namespace RobloxCS.Passes.Visitors;

public sealed class HirPrinter : HirVisitor {
    private const string Guide = "[grey]··[/]";

    private int _depth;

    public override void VisitModule(HirModule node) {
        Line($"[blue]module[/] [white]{Escape(node.SourcePath)}[/]");

        Nested(() => base.VisitModule(node));
    }

    public override void VisitStatement(HirStatement node) {
        try {
            base.VisitStatement(node);
        } catch (InvalidOperationException) {
            Line($"[red]unknown statement[/] [white]{node.GetType().Name}[/]");
        }
    }

    public override void VisitExpression(HirExpression node) {
        try {
            base.VisitExpression(node);
        } catch (InvalidOperationException) {
            Line($"[red]unknown expression[/] [white]{node.GetType().Name}[/]");
        }
    }

    public override void VisitInterpolationPart(HirInterpolationPart node) {
        try {
            base.VisitInterpolationPart(node);
        } catch (InvalidOperationException) {
            Line($"[red]unknown interpolation part[/] [white]{node.GetType().Name}[/]");
        }
    }

    protected override void VisitType(HirType node) {
        var baseType = node.Base is not null ? $" [grey]: {Type(node.Base)}[/]" : string.Empty;
        var flags = Flags(node.Symbol.IsStatic ? "static" : null, node.Symbol.IsSealed ? "sealed" : null);

        Line($"[blue]class[/] [white]{Escape(node.Symbol.Name)}[/]{baseType}{flags}");

        Nested(() => base.VisitType(node));
    }

    protected override void VisitField(HirField node) {
        var flags = Flags(
            node.IsStatic ? "static" : null,
            node.Symbol.IsConst ? "const" : null,
            node.Symbol.IsReadOnly ? "readonly" : null
        );

        Line($"[blue]field[/] [white]{Escape(node.Symbol.Name)}[/] [grey]{Type(node.Symbol.Type)}[/]{flags}");

        Nested(() => base.VisitField(node));
    }

    protected override void VisitMethod(HirMethod node) {
        var flags = Flags(
            node.IsStatic ? "static" : null,
            node.IsConstructor ? "constructor" : null,
            node.IsImplicit ? "implicit" : null,
            node.IsEntryPoint ? "entry point" : null
        );

        Line($"[blue]method[/] [white]{Escape(node.Symbol.Name)}[/] [grey]-> {Type(node.Symbol.ReturnType)}[/]{flags}");

        Nested(() => {
            base.VisitMethod(node);

            if (node.Block is null) {
                Line("[grey italic]no block[/]");
            }
        });
    }

    protected override void VisitProperty(HirProperty node) {
        var flags = Flags(
            node.IsStatic ? "static" : null,
            node.IsAuto ? "auto" : null
        );

        Line($"[blue]property[/] [white]{Escape(node.Symbol.Name)}[/] [grey]{Type(node.Symbol.Type)}[/]{flags}");

        Nested(() => {
            if (node.Initializer is not null) {
                Section("init", () => VisitExpression(node.Initializer));
            }

            if (node.Getter is not null) {
                Section("get", () => VisitBlock(node.Getter));
            }

            if (node.Setter is not null) {
                Section("set", () => VisitBlock(node.Setter));
            }
        });
    }

    protected override void VisitParameter(HirParameter node) {
        var flags = Flags(
            node.IsParams ? "params" : null,
            node.RefKind is not RefKind.None ? node.RefKind.ToString().ToLowerInvariant() : null
        );

        Line($"[blue]parameter[/] [white]{Escape(node.Symbol.Name)}[/] [grey]{Type(node.Symbol.Type)}[/]{flags}");

        if (node.DefaultValue is not null) {
            Nested(() => Section("default", () => VisitExpression(node.DefaultValue)));
        }
    }

    protected override void VisitBlock(HirBlock node) {
        Line("[cyan]block[/]");

        Nested(() => {
            if (node.Locals.Length > 0) {
                Section("locals", () => {
                    foreach (var local in node.Locals) {
                        Line($"[white]{Escape(local.Name)}[/] [grey]{Type(local.Type)}[/]");
                    }
                });
            }

            base.VisitBlock(node);
        });
    }

    protected override void VisitLocalDeclaration(HirLocalDeclaration node) {
        base.VisitLocalDeclaration(node);
    }

    protected override void VisitVariableDeclarator(HirVariableDeclarator node) {
        var uninitialized = node.Initializer is null ? " [grey italic]uninitialized[/]" : string.Empty;

        Line($"[cyan]declare[/] [white]{Escape(node.Symbol.Name)}[/] [grey]{Type(node.Symbol.Type)}[/]{uninitialized}");

        Nested(() => base.VisitVariableDeclarator(node));
    }

    protected override void VisitAssignment(HirAssignment node) {
        Line("[cyan]assign[/]");

        Nested(() => {
            Section("target", () => VisitExpression(node.Target));
            Section("value", () => VisitExpression(node.Value));
        });
    }

    protected override void VisitCompoundAssignment(HirCompoundAssignment node) {
        var flags = Flags(node.UserDefinedOperator is not null ? "user-defined" : null);

        Line($"[cyan]assign[/] [yellow]{Escape(BinaryOperatorText(node.Op))}=[/]{flags}");

        Nested(() => {
            Section("target", () => VisitExpression(node.Target));
            Section("value", () => VisitExpression(node.Value));
        });
    }

    protected override void VisitExpressionStatement(HirExpressionStatement node) {
        base.VisitExpressionStatement(node);
    }

    protected override void VisitIf(HirIf node) {
        Line("[magenta]if[/]");

        Nested(() => {
            Section("cond", () => VisitExpression(node.Condition));
            Section("then", () => VisitBlock(node.Then));

            if (node.Else is not null) {
                Section("else", () => VisitStatement(node.Else));
            }
        });
    }

    protected override void VisitWhile(HirWhile node) {
        Line("[magenta]while[/]");

        Nested(() => {
            Section("cond", () => VisitExpression(node.Condition));
            Section("body", () => VisitBlock(node.Body));
        });
    }

    protected override void VisitDoWhile(HirDoWhile node) {
        Line("[magenta]do-while[/]");

        Nested(() => {
            Section("body", () => VisitBlock(node.Body));
            Section("cond", () => VisitExpression(node.Condition));
        });
    }

    protected override void VisitFor(HirFor node) {
        Line("[magenta]for[/]");

        Nested(() => {
            if (node.Initializers.Length > 0) {
                Section("init", () => {
                    foreach (var initializer in node.Initializers) {
                        VisitStatement(initializer);
                    }
                });
            }

            if (node.Condition is not null) {
                Section("cond", () => VisitExpression(node.Condition));
            } else {
                Line("[grey italic]no condition[/]");
            }

            if (node.Incrementors.Length > 0) {
                Section("step", () => {
                    foreach (var incrementor in node.Incrementors) {
                        VisitStatement(incrementor);
                    }
                });
            }

            Section("body", () => VisitBlock(node.Body));
        });
    }

    protected override void VisitReturn(HirReturn node) {
        Line("[magenta]return[/]");

        Nested(() => base.VisitReturn(node));
    }

    protected override void VisitBreak(HirBreak node) {
        Line("[magenta]break[/]");
    }

    protected override void VisitContinue(HirContinue node) {
        Line("[magenta]continue[/]");
    }

    protected override void VisitErrorStatement(HirErrorStatement node) {
        Line("[red]error statement[/]");
    }

    protected override void VisitArgument(HirArgument node) {
        Line($"[grey]arg[/] [white]{Escape(node.Symbol.Name)}[/][grey]:[/]");

        Nested(() => base.VisitArgument(node));
    }

    protected override void VisitCall(HirCall node) {
        var target = node.Method.IsStatic
            ? $"[grey]{Type(node.Method.ContainingType)}.[/][white]{Escape(node.Method.Name)}[/]"
            : $"[white]{Escape(node.Method.Name)}[/]";

        var flags = Flags(
            node.Method.IsStatic ? "static" : null,
            node.IsExtension ? "extension" : null
        );

        Line($"[cyan]call[/] {target} [grey]-> {Type(node.Method.ReturnType)}[/]{flags}");

        Nested(() => {
            if (node.Receiver is not null) {
                Section("receiver", () => VisitExpression(node.Receiver));
            }

            foreach (var argument in node.Arguments) {
                VisitArgument(argument);
            }
        });
    }

    protected override void VisitObjectCreation(HirObjectCreation node) {
        Line($"[cyan]new[/] [white]{Type(node.Constructor.ContainingType)}[/]");

        Nested(() => base.VisitObjectCreation(node));
    }

    protected override void VisitBinary(HirBinary node) {
        var flags = Flags(
            node.IsChecked ? "checked" : null,
            node.OperatorMethod is not null ? "user-defined" : null
        );

        Line($"[cyan]binary[/] [yellow]{Escape(BinaryOperatorText(node.Op))}[/] [grey]-> {Type(node.Type)}[/]{flags}");

        Nested(() => base.VisitBinary(node));
    }

    protected override void VisitUnary(HirUnary node) {
        var flags = Flags(node.OperatorMethod is not null ? "user-defined" : null);

        Line($"[cyan]unary[/] [yellow]{Escape(UnaryOperatorText(node.Op))}[/] [grey]-> {Type(node.Type)}[/]{flags}");

        Nested(() => base.VisitUnary(node));
    }

    protected override void VisitIncrementDecrement(HirIncrementDecrement node) {
        var op = node.IsIncrement ? "++" : "--";
        var position = node.IsPrefix ? "prefix" : "postfix";

        Line($"[cyan]{position}[/] [yellow]{op}[/]");

        Nested(() => base.VisitIncrementDecrement(node));
    }

    protected override void VisitConditional(HirConditional node) {
        Line($"[cyan]ternary[/] [grey]-> {Type(node.Type)}[/]");

        Nested(() => {
            Section("cond", () => VisitExpression(node.Condition));
            Section("true", () => VisitExpression(node.WhenTrue));
            Section("false", () => VisitExpression(node.WhenFalse));
        });
    }

    protected override void VisitConversion(HirConversion node) {
        var flags = Flags(
            node.IsImplicit ? "implicit" : "explicit",
            node.Method is not null ? "user-defined" : null
        );

        Line($"[cyan]convert[/] [yellow]{node.Kind.ToString().ToLowerInvariant()}[/] [grey]{Type(node.Operand.Type)} -> {Type(node.Type)}[/]{flags}");

        Nested(() => base.VisitConversion(node));
    }

    protected override void VisitDefault(HirDefault node) {
        Line($"[cyan]default[/] [grey]{Type(node.Type)}[/]");
    }

    protected override void VisitExpressionError(HirExpressionError node) {
        Line("[red]error expression[/]");
    }

    protected override void VisitCollectionLiteral(HirCollectionLiteral node) {
        Line($"[cyan]collection[/] [grey]{Type(node.Type)}[/] [grey italic]{node.Elements.Length} elements[/]");

        Nested(() => base.VisitCollectionLiteral(node));
    }

    protected override void VisitIndexAccess(HirIndexAccess node) {
        var indexer = node.Indexer is not null ? $" [grey]{Escape(node.Indexer.ToDisplayString(SymbolDisplayFormat.MinimallyQualifiedFormat))}[/]" : string.Empty;

        Line($"[cyan]index[/]{indexer} [grey]-> {Type(node.Type)}[/]");

        Nested(() => {
            Section("receiver", () => VisitExpression(node.Receiver));

            foreach (var argument in node.Arguments) {
                VisitArgument(argument);
            }
        });
    }

    protected override void VisitInterpolatedString(HirInterpolatedString node) {
        Line("[cyan]interpolated string[/]");

        Nested(() => base.VisitInterpolatedString(node));
    }

    protected override void VisitInterpolationText(HirInterpolationText node) {
        Line($"[grey]text[/] [green]\"{Escape(node.Value)}\"[/]");
    }

    protected override void VisitInterpolationExpression(HirInterpolationExpression node) {
        var format = node.Format is not null ? $" [grey]:{Escape(node.Format)}[/]" : string.Empty;

        Line($"[grey]hole[/]{format}");

        Nested(() => base.VisitInterpolationExpression(node));
    }

    protected override void VisitLambda(HirLambda node) {
        var flags = Flags(node.Symbol.IsStatic ? "static" : null);

        Line($"[cyan]lambda[/] [grey]-> {Type(node.Symbol.ReturnType)}[/]{flags}");

        Nested(() => {
            foreach (var parameter in node.Parameters) {
                VisitParameter(parameter);
            }

            Section("body", () => VisitBlock(node.Body));
        });
    }

    protected override void VisitFieldAccess(HirFieldAccess node) {
        Line($"[cyan]field[/] {MemberTarget(node.Symbol)}");

        Nested(() => base.VisitFieldAccess(node));
    }

    protected override void VisitPropertyAccess(HirPropertyAccess node) {
        Line($"[cyan]property[/] {MemberTarget(node.Symbol)} [grey]{Type(node.Type)}[/]");

        Nested(() => base.VisitPropertyAccess(node));
    }

    protected override void VisitLiteral(HirLiteral node) {
        Line($"[cyan]literal[/] {LiteralValue(node.Value)} [grey]{Type(node.Type)}[/]");
    }

    protected override void VisitLocalRef(HirLocalRef node) {
        Line($"[cyan]local[/] [white]{Escape(node.Symbol.Name)}[/]");
    }

    protected override void VisitParameterRef(HirParameterRef node) {
        Line($"[cyan]param[/] [white]{Escape(node.Symbol.Name)}[/]");
    }

    protected override void VisitThis(HirThis node) {
        Line("[cyan]this[/]");
    }

    private void Line(string markup) {
        AnsiConsole.MarkupLine($"{Padding()}{markup}");
    }

    private void Nested(Action body) {
        _depth++;

        try {
            body();
        } finally {
            _depth--;
        }
    }

    private void Section(string label, Action body) {
        Line($"[grey italic]{label}[/]");
        Nested(body);
    }

    private string Padding() => string.Concat(Enumerable.Repeat(Guide, _depth));

    private static string MemberTarget(ISymbol member) {
        return member.IsStatic
            ? $"[grey]{Type(member.ContainingType)}.[/][white]{Escape(member.Name)}[/]"
            : $"[white]{Escape(member.Name)}[/]";
    }

    private static string Flags(params string?[] flags) {
        var present = flags.Where(flag => flag is not null).ToList();
        if (present.Count == 0) {
            return string.Empty;
        }

        return $" [grey italic]{string.Join(" ", present)}[/]";
    }

    private static string LiteralValue(object? value) => value switch {
        null => "[orange1]null[/]",
        bool b => $"[orange1]{(b ? "true" : "false")}[/]",
        string s => $"[green]\"{Escape(s)}\"[/]",
        char c => $"[green]'{Escape(c)}'[/]",

        _ => $"[yellow]{Escape(value)}[/]",
    };

    private static string BinaryOperatorText(BinaryOperatorKind kind) => kind switch {
        BinaryOperatorKind.Add => "+",
        BinaryOperatorKind.Subtract => "-",
        BinaryOperatorKind.Multiply => "*",
        BinaryOperatorKind.Divide => "/",
        BinaryOperatorKind.Remainder => "%",
        BinaryOperatorKind.LeftShift => "<<",
        BinaryOperatorKind.RightShift => ">>",
        BinaryOperatorKind.UnsignedRightShift => ">>>",
        BinaryOperatorKind.And => "&",
        BinaryOperatorKind.Or => "|",
        BinaryOperatorKind.ExclusiveOr => "^",
        BinaryOperatorKind.ConditionalAnd => "&&",
        BinaryOperatorKind.ConditionalOr => "||",
        BinaryOperatorKind.Equals => "==",
        BinaryOperatorKind.NotEquals => "!=",
        BinaryOperatorKind.LessThan => "<",
        BinaryOperatorKind.LessThanOrEqual => "<=",
        BinaryOperatorKind.GreaterThan => ">",
        BinaryOperatorKind.GreaterThanOrEqual => ">=",

        _ => kind.ToString(),
    };

    private static string UnaryOperatorText(UnaryOperatorKind kind) => kind switch {
        UnaryOperatorKind.BitwiseNegation => "~",
        UnaryOperatorKind.Not => "!",
        UnaryOperatorKind.Plus => "+",
        UnaryOperatorKind.Minus => "-",
        UnaryOperatorKind.Hat => "^",

        _ => kind.ToString(),
    };

    private static string Type(ISymbol? symbol) =>
        Escape(symbol?.ToDisplayString(SymbolDisplayFormat.MinimallyQualifiedFormat));

    private static string Escape(object? value) => Markup.Escape(value?.ToString() ?? "?");
}