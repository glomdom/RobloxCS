using RobloxCS.Common;
using Serilog;
using Serilog.Events;

namespace RobloxCS.TypeGenerator;

internal static class Program {
    private const string BaseUrl = "https://setup.rbxcdn.com";
    private const string VersionHashUrl = $"{BaseUrl}/versionQTStudio";
    
    internal static async Task Main(string[] args) {
        LoggerSetup.LevelSwitch.MinimumLevel = LogEventLevel.Verbose;
        LoggerSetup.Configure();
        
        using var http = new HttpClient();
        var versionHash = await http.GetStringAsync(VersionHashUrl);
            
        Log.Information("Generating API dump for {VersionHash}", versionHash);

        var apiDumpUrl = $"{BaseUrl}/{versionHash}-Full-API-Dump.json";
        var apiDumpJson = await http.GetStringAsync(apiDumpUrl);
        
        Log.Information("Received api dump {JSON}", apiDumpJson);
    }
}