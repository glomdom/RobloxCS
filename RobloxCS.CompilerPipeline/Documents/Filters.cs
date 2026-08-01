using Microsoft.CodeAnalysis;

namespace RobloxCS.CompilerPipeline.Documents;

public static class Filters {
    public static bool ShouldCompile(Document x) {
        if (x.Folders is ["obj", ..] or ["bin", ..]) return false;
        if (string.IsNullOrEmpty(x.FilePath)) return false;

        return true;
    }
}