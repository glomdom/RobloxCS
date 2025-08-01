using System.Text;
using RobloxCS.AST.Expressions;
using RobloxCS.AST.Prefixes;
using RobloxCS.AST.Statements;
using RobloxCS.AST.Suffixes;
using Spectre.Console;

namespace RobloxCS.AST;

public static class Dumper {
    public static void Dump(AstNode node, IAnsiConsole console, int indent = 0) {
        var sb = new StringBuilder();
        
        InternalDump(node, sb, indent);
        console.MarkupLine(sb.ToString());
    }
    
    private static void InternalDump(AstNode node, StringBuilder sb, int indent = 0) {
        switch (node) {
            #region Expressions

            case TableConstructor table: {
                sb.AppendIndentedLine("TableConstructor {", indent);
                table.Fields.ForEach(f => InternalDump(f, sb, indent + 2));
                sb.AppendIndentedLine("}", indent);

                break;
            }

            case NoKey field: {
                sb.AppendIndented("NoKey(", indent);
                InternalDump(field.Expression, sb, indent + 2);
                sb.AppendIndentedLine(")");

                break;
            }

            case StringExpression str: {
                sb.AppendIndented($"[green]\"{str.Value}\"[/]");

                break;
            }
            
            #endregion
            #region Prefixes
            
            case NamePrefix np: {
                sb.AppendIndented($"NamePrefix([green]\"{np}\"[/])");
                
                break;
            }
            
            
            #endregion
            #region Statements
            
            case FunctionCall f: {
                sb.AppendIndentedLine("FunctionCall {", indent);
                sb.AppendIndented("Prefix = ", indent + 2);
                InternalDump(f.Prefix, sb, indent + 2);
                sb.AppendLine();
                sb.AppendIndentedLine("Suffixes = [[", indent + 2);

                foreach (var suffix in f.Suffixes) {
                    InternalDump(suffix, sb, indent + 4);
                }
                
                sb.AppendIndentedLine("]]", indent + 2);
                sb.AppendIndentedLine("}", indent);
                
                break;
            }
            
            case Return ret: {
                sb.AppendIndentedLine("Return {", indent);
                ret.Returns.ForEach(r => InternalDump(r, sb, indent + 2));
                sb.AppendIndentedLine("}", indent);

                break;
            }
            
            #endregion
            #region Suffixes

            case AnonymousCall ac: {
                sb.AppendIndentedLine("AnonymousCall {", indent);
                sb.AppendIndentedLine("Arguments = [[", indent + 2);

                foreach (var expr in ac.Arguments.Arguments) {
                    InternalDump(expr, sb, indent + 4);
                }
                
                sb.AppendIndentedLine("]]", indent + 2);
                sb.AppendIndentedLine("}", indent);
                
                break;
            }

            #endregion

            default: {
                sb.AppendIndentedLine($"[red]dump method not implemented for {node.GetType().Name}[/]", indent);

                break;
            }
        }
    }

    private static string SpaceIndent(int indent = 0) {
        return string.Concat(Enumerable.Repeat(" ", indent));
    }

    private static StringBuilder AppendIndentedLine(this StringBuilder builder, string line, int indent = 0) {
        return builder.AppendLine($"{SpaceIndent(indent)}{line}");
    }
    
    private static StringBuilder AppendIndented(this StringBuilder builder, string line, int indent = 0) {
        return builder.Append($"{SpaceIndent(indent)}{line}");
    }
}