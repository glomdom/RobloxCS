using System.Diagnostics;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;
using RobloxCS.Common;
using RobloxCS.TypeGenerator.Models;
using Serilog;
using Serilog.Events;

namespace RobloxCS.TypeGenerator;

internal static class Program {
    private const string BaseUrl = "https://setup.rbxcdn.com";
    private const string VersionHashUrl = $"{BaseUrl}/versionQTStudio";
    private const string ApiDumpUrl = $"https://raw.githubusercontent.com/MaximumADHD/Roblox-Client-Tracker/refs/heads/roblox/Mini-API-Dump.json";

    private static readonly List<string> SkippedNames = ["Studio", "BindableFunction"];
    private static readonly List<string> EnumNames = [];

    // FIXME: im too lazy to do all of these so we hardcode return LuaTuple
    private static readonly HashSet<string> PatchedMethods = [
        "AssetService.PromptCreateAssetAsync",
        "BasePart.CanSetNetworkOwnership",
        "Camera.WorldToScreenPoint",
        "Camera.WorldToViewportPoint",
        "Chat.InvokeChatCallback",
        "GroupService.GetGroupInfoAsync",
        "GuiService.GetGuiInset",
        "HapticService.GetMotor",
        "HttpService.JSONDecode",
        "SoundService.GetListener",
        "TeleportService.GetLocalPlayerTeleportData",
        "TeleportService.GetTeleportSetting",
        "TeleportService.GetPlayerPlaceInstanceAsync",
        "TeleportService.ReserveServer",
        "UserInputService.GetDeviceRotation",
    ];

    private static readonly JsonSerializerOptions Options = new() {
        Converters = { new JsonStringEnumConverter() },
    };

    internal static async Task Main() {
        LoggerSetup.LevelSwitch.MinimumLevel = LogEventLevel.Verbose;
        LoggerSetup.Configure();

        var api = await DownloadAndDeserializeAsync();
        var workingDirectory = Environment.CurrentDirectory;
        var generatedDirectory = Path.Combine(workingDirectory, "Generated");

        Log.Information("Starting file generation in {CurrentDir}", generatedDirectory);
        Log.Debug("Creating directory for output");
        Directory.CreateDirectory(generatedDirectory);

        var enumFiles = GenerateEnumFiles(api);
        var classFiles = GenerateClassFiles(api);

        await WritePairPathSourceAsync(generatedDirectory, enumFiles);
        await WritePairPathSourceAsync(generatedDirectory, classFiles);
    }

    private static async Task WritePairPathSourceAsync(string path, Dictionary<string, string> pair) {
        foreach (var (fileName, source) in pair) {
            await File.WriteAllTextAsync(Path.Combine(path, fileName), source);
        }
    }

    private static Dictionary<string, string> GenerateEnumFiles(RobloxApiDump api) {
        var result = new Dictionary<string, string>();
        var builder = new StringBuilder();

        var watch = Stopwatch.StartNew();
        Log.Information("Starting enum generation");

        foreach (var enumDef in api.Enums) {
            builder.AppendLine("namespace RobloxCS.Types;");
            builder.AppendLine("public static partial class Enums {");
            builder.AppendLine($"    public enum {enumDef.Name} {{");

            foreach (var item in enumDef.Items) {
                builder.AppendLine($"        {item.Name} = {item.Value},");
            }

            builder.AppendLine("    }");
            builder.AppendLine("}");

            EnumNames.Add(enumDef.Name);
            result[$"Enum{enumDef.Name}.g.cs"] = builder.ToString();
            builder.Clear();
        }

        watch.Stop();
        Log.Information("Finished enum generation in {ElapsedMS}ms", watch.ElapsedMilliseconds);

        return result;
    }

    private static Dictionary<string, string> GenerateClassFiles(RobloxApiDump api) {
        var result = new Dictionary<string, string>();
        var builder = new StringBuilder();

        var watch = Stopwatch.StartNew();
        Log.Information("Starting class generation");

        var generatedMemberCount = 0;
        foreach (var classDef in api.Classes) {
            if (!IsClassAllowed(classDef)) continue;

            var shouldAppendService = false;

            if (SkippedNames.Contains(classDef.Name)) {
                Log.Verbose("Skipping class {ClassName} with tags {Tags}", classDef.Name, classDef.Tags);

                continue;
            }

            if (!classDef.Name.EndsWith("Service")) {
                shouldAppendService = true;
            }

            Log.Verbose("Generating class {ClassName} with tags {Tags}", classDef.Name, classDef.Tags);

            var isService = IsService(classDef);

            builder.AppendLine("namespace RobloxCS.Types;");

            if (isService && classDef.Name != "Workspace") {
                builder.AppendLine($"[RobloxNative(\"{classDef.Name}\", RobloxNativeType.Service)]");
                builder.AppendLine($"public static partial class {classDef.Name}{(shouldAppendService ? "Service" : string.Empty)} {{");
            } else {
                builder.AppendLine($"[RobloxNative(\"{classDef.Name}\", RobloxNativeType.Instance)]");
                builder.AppendLine($"public partial class {classDef.Name} {{");
            }

            foreach (var member in classDef.Members) {
                if (!IsMemberAllowed(member)) continue;

                if (member.MemberType is RobloxMemberType.Property) {
                    var prop = (RobloxProperty)member;

                    Log.Verbose("Generating property {PropName} with tags {Tags} and security {Security}", prop.Name, prop.Tags, prop.Security);
                    var propType = RobloxTypeToCSharp(prop.ValueType);

                    if (isService) {
                        var normalized = prop.Name.Replace(" ", string.Empty);
                        if (prop.Name != normalized) {
                            builder.AppendLine($"    [RobloxName(\"{prop.Name}\")]");
                            builder.AppendLine($"    public static {propType} {normalized} {{ get; }} = default!;");
                        } else {
                            builder.AppendLine($"    public static {propType} {prop.Name} {{ get; }} = default!;");
                        }
                    } else {
                        if (prop.Name == classDef.Name) {
                            builder.AppendLine($"    [RobloxName(\"{prop.Name}\")]");
                            builder.AppendLine($"    public {propType} Value {{ get; }} = default!;");
                        } else {
                            var normalized = prop.Name.Replace(" ", string.Empty);
                            if (prop.Name != normalized) {
                                builder.AppendLine($"    [RobloxName(\"{prop.Name}\")]");
                                builder.AppendLine($"    public {propType} {normalized} {{ get; }} = default!;");
                            } else {
                                builder.AppendLine($"    public {propType} {prop.Name} {{ get; }} = default!;");
                            }
                        }
                    }

                    generatedMemberCount++;
                }

                if (member.MemberType is RobloxMemberType.Function) {
                    if (PatchedMethods.Contains($"{classDef.Name}.{member.Name}")) {
                        Log.Verbose("Skipping function {FunctionName} as it is patched", member.Name);

                        continue;
                    }

                    var prop = (RobloxFunction)member;
                    var returnType = RobloxTypeToCSharp(prop.ReturnType);

                    Log.Verbose("Generating function {FunctionName} with tags {Tags} and security {Security}", prop.Name, prop.Tags, prop.Security);

                    if (isService) {
                        builder.AppendLine(
                            $"    public static {returnType} {prop.Name}() => throw new InvalidOperationException(\"Cannot call reserved method for RobloxCS transpiler.\");"
                        );
                    } else {
                        builder.AppendLine(
                            $"    public {returnType} {prop.Name}() => throw new InvalidOperationException(\"Cannot call reserved method for RobloxCS transpiler.\");"
                        );
                    }

                    generatedMemberCount++;
                }
            }

            if (generatedMemberCount == 0 && classDef.Members.Count != 0) {
                builder.Clear();
                generatedMemberCount = 0;

                continue;
            }

            builder.AppendLine("}");

            result[$"Class{classDef.Name}{(shouldAppendService ? "Service" : string.Empty)}.g.cs"] = builder.ToString();
            builder.Clear();
            generatedMemberCount = 0;
        }

        watch.Stop();
        Log.Information("Finished class generation in {ElapsedMS}ms", watch.ElapsedMilliseconds);

        return result;
    }

    private static bool IsClassAllowed(RobloxClass member) =>
        member.Tags == null || member.Tags.Any(t => t.EnumValue is RobloxTagKind.Hidden or RobloxTagKind.NotScriptable or RobloxTagKind.Deprecated);

    private static bool IsMemberAllowed(RobloxMember member) {
        if (member.Tags == null) return member.Security is not { Read: not RobloxSecurityType.None, Write: not RobloxSecurityType.None };
        if (member.Tags.Any(t => t.EnumValue is RobloxTagKind.Hidden or RobloxTagKind.NotScriptable or RobloxTagKind.Deprecated)) return false;

        return member.Security is not { Read: not RobloxSecurityType.None, Write: not RobloxSecurityType.None };
    }

    private static string RobloxTypeToCSharp(RobloxType type) {
        if (EnumNames.Contains(type.Name)) {
            return $"Enums.{type.Name}";
        }

        return type.Name switch {
            "string" => "string",
            "int" => "int",
            "int64" => "int",
            "bool" => "bool",
            "Content" => "string",
            "ProtectedString" => "string",
            "BinaryString" => "string",
            "null" => "void",
            "Dictionary" => "object",
            "Objects" => "List<Instance>",
            "Tuple" => "LuaTuple",
            "Variant" => "object",

            _ => type.Name,
        };
    }

    // horribly optimized functions below especially cause its being called in a hot spot but i couldnt care less
    private static bool IsService(RobloxClass cls) {
        return cls.Tags is not null && cls.Tags.Any(t => t.EnumValue == RobloxTagKind.Service);
    }

    private static async Task<RobloxApiDump> DownloadAndDeserializeAsync() {
        using var http = new HttpClient();
        var apiDumpJson = await http.GetStringAsync(ApiDumpUrl);
        await File.WriteAllTextAsync("out.json", apiDumpJson);

        var watch = Stopwatch.StartNew();
        Log.Information("Starting deserialization");

        using var stream = new MemoryStream(Encoding.UTF8.GetBytes(apiDumpJson));

        var output = await JsonSerializer.DeserializeAsync<RobloxApiDump>(stream, Options);
        if (output is null) throw new Exception("Failed to deserialize API dump");

        watch.Stop();

        Log.Information("Done! {ClassCount} classes, {EnumCount} enums in {ElapsedMS}ms", output.Classes.Count, output.Enums.Count, watch.ElapsedMilliseconds);

        return output;
    }
}