using RobloxCS.HIR.Expressions;

namespace RobloxCS.Lowering;

/// <summary>
/// Lowering-only node: an existing array or list passed as a Roblox API's <c>params</c> argument.
/// Created by <c>NativeArguments</c> and is never seen by HIR passes.
/// </summary>
internal sealed record HirNativeSpread : HirExpression {
    public required HirExpression Collection { get; init; }
}
