using RobloxCS.AST;

namespace RobloxCS.Transpiler.Walkers;

public sealed class ServiceLoweringWalker : AstRewriter {
    // private readonly Dictionary<string, Expression> _serviceUsageMap = new();
    //
    // public AstNode VisitTransientServiceUsageExpression(TransientServiceUsageExpression node) {
    //     Log.Debug("Lowering transient service usage");
    //     
    //     var getServiceStmt = new FunctionCallExpression {
    //         Prefix = NamePrefix.FromString("game"),
    //         Suffixes = [
    //             new MethodCall {
    //                 Name = "GetService",
    //                 Args = ExpressionHelpers.FunctionArgsFromExpression(new StringExpression { Value = node.ServiceName }),
    //             },
    //         ],
    //     };
    //
    //     if (node.AccessExpression is FunctionCallExpression funcCall) {
    //         if (funcCall.Prefix is not NamePrefix name) throw new Exception("Cannot lower transient server usage whose call is a function whose prefix is not a NamePrefix.");
    //
    //         var call = new FunctionCallExpression {
    //             Prefix = new NamePrefix { Name = node.ServiceName },
    //             Suffixes = [
    //                 new Dot {
    //                     Name = SymbolExpression.FromString(name.Name),
    //                 },
    //                 ..funcCall.Suffixes,
    //             ],
    //         };
    //
    //         _serviceUsageMap[node.ServiceName] = getServiceStmt;
    //
    //         return call;
    //     }
    //
    //     _serviceUsageMap[node.ServiceName] = getServiceStmt;
    //
    //     return SymbolExpression.FromString(node.ServiceName);
    // }
    //
    // public List<Statement> GetServiceStatements() {
    //     var stmts = new List<Statement>();
    //
    //     foreach (var (varName, useExpression) in _serviceUsageMap) {
    //         var assignment = StatementHelpers.UntypedLocalAssignment(varName, useExpression);
    //
    //         stmts.Add(assignment);
    //     }
    //
    //     return stmts;
    // }
}