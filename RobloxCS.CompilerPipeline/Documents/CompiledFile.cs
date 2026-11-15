namespace RobloxCS.CompilerPipeline.Documents;

public sealed record CompiledFile(string OutPath, string Code) {
    public bool UsesCollections { get; init; }
}
