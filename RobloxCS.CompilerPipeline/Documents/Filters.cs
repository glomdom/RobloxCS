using Microsoft.CodeAnalysis;

namespace RobloxCS.CompilerPipeline.Documents;

public static class Filters {
    public static bool ShouldCompile(Document x) => x.Folders is not ["obj", ..];
}