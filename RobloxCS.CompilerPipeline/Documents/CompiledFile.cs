using System.Collections.Immutable;

namespace RobloxCS.CompilerPipeline.Documents;

public sealed record CompiledFile(string OutPath, string Code, ImmutableArray<string> Diagnostics);