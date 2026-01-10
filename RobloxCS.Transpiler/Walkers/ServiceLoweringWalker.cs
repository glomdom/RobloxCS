using RobloxCS.AST;
using RobloxCS.AST.Expressions;
using RobloxCS.AST.Functions;
using RobloxCS.AST.Prefixes;
using RobloxCS.AST.Statements;
using RobloxCS.AST.Suffixes;
using RobloxCS.AST.Transient;
using RobloxCS.Transpiler.Helpers;
using Serilog;

namespace RobloxCS.Transpiler.Walkers;

public sealed class ServiceLoweringWalker : AstRewriter, IInternalAstVisitor<AstNode> {
    private readonly Dictionary<string, Expression> _serviceUsageMap = new();

    public AstNode VisitTransientServiceUsageExpression(TransientServiceUsageExpression node) {
        Log.Debug("Lowering transient service usage");

        if (node.AccessExpression is FunctionCallExpression funcCall) {
            if (funcCall.Prefix is not NamePrefix name) throw new Exception("Cannot lower transient server usage whose call is a function whose prefix is not a NamePrefix.");

            var call = new FunctionCallExpression {
                Prefix = new NamePrefix { Name = node.ServiceName },
                Suffixes = [
                    new Dot {
                        Name = SymbolExpression.FromString(name.Name),
                    },
                    ..funcCall.Suffixes,
                ],
            };

            _serviceUsageMap[node.ServiceName] = new FunctionCallExpression {
                Prefix = NamePrefix.FromString("game"),
                Suffixes = [
                    new MethodCall {
                        Name = "GetService",
                        Args = ExpressionHelpers.FunctionArgsFromExpression(new StringExpression { Value = node.ServiceName }),
                    },
                ],
            };

            return call;
        }

        return SymbolExpression.FromString(node.ServiceName);
    }

    public List<Statement> GetServiceStatements() {
        var stmts = new List<Statement>();

        foreach (var (varName, useExpression) in _serviceUsageMap) {
            var assignment = StatementHelpers.UntypedLocalAssignment(varName, useExpression);

            stmts.Add(assignment);
        }

        return stmts;
    }
}