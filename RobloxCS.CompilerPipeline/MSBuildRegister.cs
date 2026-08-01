using Microsoft.Build.Locator;

namespace RobloxCS.CompilerPipeline;

public static class MsBuildRegister {
    public static void RegisterDefaults() {
        if (!MSBuildLocator.IsRegistered) {
            var vsi = MSBuildLocator.RegisterDefaults();
        }
    }
}