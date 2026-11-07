using System.Reflection;
using System.Text;
using RobloxCS.CLI.Commands.Build;
using RobloxCS.CLI.Commands.Emit;
using RobloxCS.CLI.Commands.Init;
using RobloxCS.CLI.Commands.Package;
using RobloxCS.CLI.Commands.Package.Add;
using RobloxCS.CLI.Commands.Package.Remove;
using RobloxCS.CLI.Commands.Package.Restore;
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
            config.SetApplicationVersion(typeof(Program).Assembly.GetCustomAttribute<AssemblyInformationalVersionAttribute>()?.InformationalVersion ?? "unk");
            config.UseStrictParsing();
            config.PropagateExceptions();

            config.AddCommand<InitCommand>("init");
            config.AddCommand<BuildCommand>("build");
            config.AddCommand<EmitCommand>("emit");

            config.AddBranch<PackageSettings>("package", package => {
                package.SetDescription("Manage project dependencies.");

                package.AddCommand<PackageAddCommand>("add").WithDescription("Add a dependency to the project.");
                package.AddCommand<PackageRemoveCommand>("remove").WithDescription("Remove a dependency from the project.");
                package.AddCommand<PackageRestoreCommand>("restore").WithDescription("Installs all added dependencies from the project.");
            }).WithAlias("pkg");
        });

        try {
            return app.Run(args);
        } catch (CommandParseException ex) {
            if (ex.Pretty is { } pretty) {
                AnsiConsole.Write(pretty);
            } else {
                AnsiConsole.MarkupLine($"[red]error:[/] {Markup.Escape(ex.Message)}");
            }

            return -1;
        } catch (CommandRuntimeException ex) {
            app.Run([..args, "--help"]);

            AnsiConsole.WriteLine();
            AnsiConsole.MarkupLine($"[red]error:[/] {Markup.Escape(ex.Message)}");

            return -1;
        } catch (CommandAppException ex) {
            if (ex.Pretty is { } pretty) {
                AnsiConsole.Write(pretty);
            } else {
                AnsiConsole.MarkupLine($"[red]error:[/] {Markup.Escape(ex.Message)}");
            }

            return -1;
        } catch (Exception ex) {
            AnsiConsole.WriteException(ex);
            AnsiConsole.MarkupLine("[cyan]note:[/] this may be a compiler bug, please report it at https://github.com/glomdom/RobloxCS");

            return -1;
        }
    }
}