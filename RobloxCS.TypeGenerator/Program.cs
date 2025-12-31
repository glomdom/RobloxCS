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

    private static readonly List<string> SkippedNames = ["Studio"];
    private static readonly List<string> EnumNames = [];

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

        foreach (var classDef in api.Classes) {
            if (SkippedNames.Contains(classDef.Name)) {
                Log.Verbose("Skipping class {ClassName} with tags {Tags}", classDef.Name, classDef.Tags);

                continue;
            }
            
            Log.Verbose("Generating class {ClassName} with tags {Tags}", classDef.Name, classDef.Tags);

            var isService = IsService(classDef);

            builder.AppendLine("namespace RobloxCS.Types;");

            if (isService) {
                builder.AppendLine($"[RobloxNative(\"{classDef.Name}\", RobloxNativeType.Service)]");
                builder.AppendLine($"public static class {classDef.Name} {{");
            } else {
                builder.AppendLine($"[RobloxNative(\"{classDef.Name}\", RobloxNativeType.Instance)]");
                builder.AppendLine($"public class {classDef.Name} {{");
            }

            foreach (var member in classDef.Members) {
                if (member.MemberType is RobloxMemberType.Property) {
                    var prop = (RobloxProperty)member;

                    if (IsHidden(prop.Tags)) {
                        Log.Verbose("Skipping property {PropName} as it is hidden.", prop.Name);

                        continue;
                    }

                    Log.Verbose("Generating property {PropName} with tags {Tags}", prop.Name, prop.Tags);

                    if (isService) {
                        var normalized = prop.Name.Replace(" ", string.Empty);
                        if (prop.Name != normalized) {
                            builder.AppendLine($"    [RobloxName(\"{prop.Name}\")]");
                            builder.AppendLine($"    public static {RobloxTypeToCSharp(prop.ValueType)} {normalized} {{ get; }} = default!;");
                        } else {
                            builder.AppendLine($"    public static {RobloxTypeToCSharp(prop.ValueType)} {prop.Name} {{ get; }} = default!;");
                        }
                    } else {
                        if (prop.Name == classDef.Name) {
                            builder.AppendLine($"    [RobloxName(\"{prop.Name}\")]");
                            builder.AppendLine($"    public {RobloxTypeToCSharp(prop.ValueType)} Value {{ get; }} = default!;");
                        } else {
                            var normalized = prop.Name.Replace(" ", string.Empty);
                            if (prop.Name != normalized) {
                                builder.AppendLine($"    [RobloxName(\"{prop.Name}\")]");
                                builder.AppendLine($"    public {RobloxTypeToCSharp(prop.ValueType)} {normalized} {{ get; }} = default!;");
                            } else {
                                builder.AppendLine($"    public {RobloxTypeToCSharp(prop.ValueType)} {prop.Name} {{ get; }} = default!;");
                            }
                        }
                    }
                }
            }

            builder.AppendLine("}");

            result[$"Class{classDef.Name}.g.cs"] = builder.ToString();
            builder.Clear();
        }

        watch.Stop();
        Log.Information("Finished class generation in {ElapsedMS}ms", watch.ElapsedMilliseconds);

        return result;
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

            _ => type.Name,
        };
    }

    // horribly optimized functions below especially cause its being called in a hot spot but i couldnt care less
    private static bool IsService(RobloxClass cls) {
        return cls.Tags is not null && cls.Tags.Contains(RobloxTagKind.Service);
    }

    public static bool IsHidden(List<RobloxTagKind>? tags) {
        return tags is not null && tags.Contains(RobloxTagKind.Hidden);
    }

    private static bool IsCreatable(RobloxClass cls) {
        if (cls.Tags is null) return true;

        return !cls.Tags.Contains(RobloxTagKind.NotCreatable);
    }

    private static async Task<RobloxApiDump> DownloadAndDeserializeAsync() {
        using var http = new HttpClient();
        var versionHash = await http.GetStringAsync(VersionHashUrl);

        Log.Information("Downloading API dump for {VersionHash}", versionHash);

        var apiDumpUrl = $"{BaseUrl}/{versionHash}-API-Dump.json";
        var apiDumpJson = await http.GetStringAsync(apiDumpUrl);

        var watch = Stopwatch.StartNew();
        Log.Information("Starting deserialization");

        using var stream = new MemoryStream(Encoding.UTF8.GetBytes(apiDumpJson));

        var output = await JsonSerializer.DeserializeAsync<RobloxApiDump>(stream, Options);
        if (output is null) throw new Exception("Failed to deserialize API dump");

        await File.WriteAllTextAsync("out.json", apiDumpJson);

        watch.Stop();

        Log.Information("Done! {ClassCount} classes, {EnumCount} enums in {ElapsedMS}ms", output.Classes.Count, output.Enums.Count, watch.ElapsedMilliseconds);

        return output;
    }
}