using System.Diagnostics.CodeAnalysis;
using RobloxCS.AST;
using RobloxCS.Common.Diagnostics;

namespace RobloxCS.Transpiler;

/// <param name="Chunk">The lowered Luau, or <c>null</c> when an error was reported.</param>
public sealed record TranspileResult(Chunk? Chunk, IReadOnlyList<Diagnostic> Diagnostics) {
    /// <summary>The output requires a runtime module (collections or reified generics).</summary>
    public bool UsesRuntime { get; init; }

    [MemberNotNullWhen(true, nameof(Chunk))]
    public bool Ok => Chunk is not null;
}
