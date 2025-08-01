using RobloxCS.CLI.Commands.Compile;
using Spectre.Console.Cli;

namespace RobloxCS.CLI;

internal static class Program {
    private static int Main(string[] args) {
        var app = new CommandApp();
        app.Configure(config => {
            config.SetApplicationName("rbxcsc");
            config.SetApplicationVersion("0.0.0a");
            config.UseStrictParsing();

            config.AddBranch<CompileSettings>("compile", branch => {
                branch.SetDescription("Compile a C# file to Luau or a C# project");
                branch.AddCommand<FileCompileCommand>("file");
            });
        });

        return app.Run(args);
    }
}