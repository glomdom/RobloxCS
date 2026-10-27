using System.Text;
using RobloxCS.CLI.Commands.Build;
using RobloxCS.CLI.Commands.Emit;
using RobloxCS.Common;
using Spectre.Console;
using Spectre.Console.Cli;

namespace RobloxCS.CLI;

internal static class Program {
    private static int Main(string[] args) {
        Console.OutputEncoding = new UTF8Encoding();
        
        LoggerSetup.Configure();

        var app = new CommandApp();
        app.Configure(config => {
            config.SetApplicationName("rbxcsc");
            config.SetApplicationVersion("0.0.0a");
            config.UseStrictParsing();
            config.PropagateExceptions();

            config.AddCommand<BuildCommand>("build")
                .WithExample("build")
                .WithExample("build", "--watch");

            config.AddCommand<EmitCommand>("emit")
                .WithExample("emit", "Main.client.cs");
        });

        try {
            return app.Run(args);
        } catch (CommandParseException ex) {
            AnsiConsole.MarkupLine($"[red]error:[/] {Markup.Escape(ex.Message)}");
            AnsiConsole.WriteLine();
            app.Run(["--help"]);

            return -1;
        } catch (Exception ex) {
            AnsiConsole.WriteLine(ex.ToString());
            AnsiConsole.MarkupLine("[cyan]note:[/] This may be a compiler bug, please report it at https://github.com/glomdom/RobloxCS");

            return -1;
        }
    }
}