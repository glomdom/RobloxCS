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

    internal static async Task Main(string[] args) {
        LoggerSetup.LevelSwitch.MinimumLevel = LogEventLevel.Verbose;
        LoggerSetup.Configure();

        using var http = new HttpClient();
        var versionHash = await http.GetStringAsync(VersionHashUrl);

        Log.Information("Downloading API dump for {VersionHash}", versionHash);

        var apiDumpUrl = $"{BaseUrl}/{versionHash}-API-Dump.json";
        var apiDumpJson = await http.GetStringAsync(apiDumpUrl);

        Log.Information("Starting deserialization");

        using var stream = new MemoryStream(Encoding.UTF8.GetBytes(apiDumpJson));

        await File.WriteAllTextAsync("out.json", apiDumpJson);

        var output = await JsonSerializer.DeserializeAsync<RobloxApiDump>(stream, Options);
        if (output is null) throw new Exception("Failed to deserialize API dump");

        Log.Information("Done! {ClassCount} classes", output.Classes.Count);

        foreach (var cls in output.Classes.Where(cls => cls.Name == "Instance")) {
            Log.Information("Class {ClassName} has {MemberCount} members", cls.Name, cls.Members.Count);

            foreach (var mem in cls.Members) {
                Log.Information("Member {Member} [{Unmapped}]", mem, mem.UnmappedData);
            }
        }
    }
}