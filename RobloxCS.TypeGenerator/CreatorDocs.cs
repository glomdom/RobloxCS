using System.Collections.Concurrent;
using RobloxCS.TypeGenerator.Models;
using Serilog;
using YamlDotNet.Core;
using YamlDotNet.Serialization;
using YamlDotNet.Serialization.NamingConventions;

namespace RobloxCS.TypeGenerator;

internal sealed class CreatorDocs {
    private const string BaseUrl = "https://raw.githubusercontent.com/Roblox/creator-docs/main/content/en-us/reference/engine/classes/";
    private static readonly TimeSpan CacheLifetime = TimeSpan.FromDays(1);

    private static readonly IDeserializer Yaml = new DeserializerBuilder()
        .WithNamingConvention(CamelCaseNamingConvention.Instance)
        .IgnoreUnmatchedProperties()
        .Build();

    private readonly Dictionary<string, DocClass> _classes;

    private CreatorDocs(Dictionary<string, DocClass> classes) {
        _classes = classes;
    }

    public static CreatorDocs Empty { get; } = new([]);

    public static async Task<CreatorDocs> LoadAsync(IEnumerable<string> classNames, string cacheDirectory) {
        Directory.CreateDirectory(cacheDirectory);

        using var http = new HttpClient();
        var classes = new ConcurrentDictionary<string, DocClass>();

        await Parallel.ForEachAsync(classNames, new ParallelOptions { MaxDegreeOfParallelism = 16 }, async (name, cancellation) => {
            var text = await ReadAsync(http, name, cacheDirectory, cancellation);
            if (text is null) {
                return;
            }

            try {
                classes[name] = Yaml.Deserialize<DocClass>(text);
            } catch (YamlException ex) {
                Log.Warning("Could not parse creator docs for {ClassName}: {Message}", name, ex.Message);
            }
        });

        Log.Information("Loaded creator docs for {Count} classes", classes.Count);

        return new CreatorDocs(new Dictionary<string, DocClass>(classes));
    }

    public DocMember? FindProperty(string className, string name) {
        return _classes.GetValueOrDefault(className)?.Properties?.FirstOrDefault(member => member.Name == $"{className}.{name}");
    }

    public DocMember? FindMethod(string className, string name) {
        return _classes.GetValueOrDefault(className)?.Methods?.FirstOrDefault(member => member.Name == $"{className}:{name}");
    }

    private static async Task<string?> ReadAsync(HttpClient http, string name, string cacheDirectory, CancellationToken cancellation) {
        var path = Path.Combine(cacheDirectory, $"{name}.yaml");

        if (File.Exists(path) && DateTime.UtcNow - File.GetLastWriteTimeUtc(path) < CacheLifetime) {
            return await File.ReadAllTextAsync(path, cancellation);
        }

        using var response = await http.GetAsync($"{BaseUrl}{name}.yaml", cancellation);
        if (!response.IsSuccessStatusCode) {
            Log.Verbose("No creator docs for {ClassName} ({Status})", name, (int)response.StatusCode);

            return null;
        }

        var text = await response.Content.ReadAsStringAsync(cancellation);
        await File.WriteAllTextAsync(path, text, cancellation);

        return text;
    }
}