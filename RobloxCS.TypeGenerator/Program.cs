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

        var classFiles = GenerateClassFiles(api);
        var enumFiles = GenerateEnumFiles(api);

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
            if (classDef.Tags is null) continue;

            builder.AppendLine("namespace RobloxCS.Types;");
            builder.AppendLine($"public class {classDef.Name} {{}}");

            if (classDef.Tags.Contains(RobloxTagKind.Service)) {
                Log.Information("Have service: {ClassName} {Tags}", classDef.Name, classDef.Tags);
            }

            result[$"Class{classDef.Name}.g.cs"] = builder.ToString();
            builder.Clear();
        }

        watch.Stop();
        Log.Information("Finished class generation in {ElapsedMS}ms", watch.ElapsedMilliseconds);

        return result;
    }

    // horribly optimized especially cause its being called in a hot spot but i couldnt care less
    private static bool IsService(RobloxClass cls) {
        ArgumentNullException.ThrowIfNull(cls.Tags);

        return cls.Tags.Contains(RobloxTagKind.Service);
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

        watch.Stop();

        Log.Information("Done! {ClassCount} classes, {EnumCount} enums in {ElapsedMS}ms", output.Classes.Count, output.Enums.Count, watch.ElapsedMilliseconds);

        return output;
    }
}