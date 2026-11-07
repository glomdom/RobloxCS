using System.Text;
using RobloxCS.Packages.Dependencies;

namespace RobloxCS.CLI.Commands.Init;

internal static class ProjectTemplate {
    public static readonly string[] WriteIfMissing = [".gitignore"];

    public static string? SanitizeName(string raw) {
        var builder = new StringBuilder();
        var upperNext = true;

        foreach (var character in raw) {
            if (!char.IsAsciiLetterOrDigit(character)) {
                upperNext = true;

                continue;
            }

            builder.Append(upperNext ? char.ToUpperInvariant(character) : character);
            upperNext = false;
        }

        if (builder.Length == 0) {
            return null;
        }

        if (char.IsAsciiDigit(builder[0])) {
            builder.Insert(0, '_');
        }

        return builder.ToString();
    }

    public static Dictionary<string, string> Files(string name, string version) {
        var files = new Dictionary<string, string> {
            [$"{name}.slnx"] = Solution,
            ["default.project.json"] = RojoProject,
            ["Directory.Build.props"] = BuildProps,
            ["rokit.toml"] = Rokit,
            ["rbxcs.toml"] = RbxcsToml,
            [".gitignore"] = GitIgnore,
            [$"src/{name}.Shared/{name}.Shared.csproj"] = SharedProject,
            [$"src/{name}.Shared/Greeter.cs"] = Greeter,
            [$"src/{name}.Server/{name}.Server.csproj"] = ServerProject,
            [$"src/{name}.Server/Main.server.cs"] = ServerMain,
            [$"src/{name}.Client/{name}.Client.csproj"] = ClientProject,
            [$"src/{name}.Client/Main.client.cs"] = ClientMain,
        };

        return files.ToDictionary(
            pair => pair.Key,
            pair => pair.Value.Replace("{{Name}}", name).Replace("{{Version}}", version)
        );
    }

    private const string Solution = """
        <Solution>
          <Project Path="src/{{Name}}.Shared/{{Name}}.Shared.csproj" />
          <Project Path="src/{{Name}}.Server/{{Name}}.Server.csproj" />
          <Project Path="src/{{Name}}.Client/{{Name}}.Client.csproj" />
        </Solution>

        """;

    private const string RojoProject = """
        {
          "name": "{{Name}}",
          "tree": {
            "$className": "DataModel",
            "ReplicatedStorage": {
              "Shared": {
                "$path": "out/shared"
              },
              "Packages": {
                "$path": { "optional": "out/packages" }
              }
            },
            "ServerScriptService": {
              "Server": {
                "$path": "out/server"
              }
            },
            "StarterPlayer": {
              "StarterPlayerScripts": {
                "Client": {
                  "$path": "out/client"
                }
              }
            }
          }
        }

        """;

    private const string BuildProps = """
        <Project>

            <Import Project="rbxcs.props" Condition="Exists('rbxcs.props')" />

        </Project>

        """;

    private const string Rokit = """
        [tools]
        rbxcsc = "glomdom/RobloxCS@{{Version}}"

        """;
    
    private const string RbxcsToml = """
        robloxcs = {{Version}}
        
        [dependencies]

        """;

    private const string GitIgnore = """
        bin/
        obj/
        out/
        sourcemap.json
        *.rbxl
        *.rbxlx

        """;

    private const string SharedProject = """
        <Project Sdk="Microsoft.NET.Sdk">

            <PropertyGroup>
                <RootNamespace>{{Name}}.Shared</RootNamespace>
            </PropertyGroup>

        </Project>

        """;

    private const string ServerProject = """
        <Project Sdk="Microsoft.NET.Sdk">

            <PropertyGroup>
                <RootNamespace>{{Name}}.Server</RootNamespace>
            </PropertyGroup>

            <ItemGroup>
                <ProjectReference Include="..\{{Name}}.Shared\{{Name}}.Shared.csproj" />
            </ItemGroup>

        </Project>

        """;

    private const string ClientProject = """
        <Project Sdk="Microsoft.NET.Sdk">

            <PropertyGroup>
                <RootNamespace>{{Name}}.Client</RootNamespace>
            </PropertyGroup>

            <ItemGroup>
                <ProjectReference Include="..\{{Name}}.Shared\{{Name}}.Shared.csproj" />
            </ItemGroup>

        </Project>

        """;

    private const string Greeter = """
        namespace {{Name}}.Shared;

        public class Greeter {
            public string Prefix { get; }

            public Greeter(string prefix) {
                Prefix = prefix;
            }

            public string Greet(string name) => Prefix + ", " + name + "!";
        }

        """;

    private const string ServerMain = """
        using {{Name}}.Shared;

        namespace {{Name}}.Server;

        internal class Main {
            [EntryPoint]
            internal static void Start() {
                var greeter = new Greeter("Welcome");

                Services.Players.PlayerAdded.Connect(player => {
                    Console.WriteLine(greeter.Greet(player.DisplayName));
                });
            }
        }

        """;

    private const string ClientMain = """
        using {{Name}}.Shared;

        namespace {{Name}}.Client;

        internal class Main {
            [EntryPoint]
            internal static void Start() {
                var greeter = new Greeter("Hello from the client");

                Console.WriteLine(greeter.Greet("client"));
            }
        }

        """;
}