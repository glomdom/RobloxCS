using System.Collections.Concurrent;
using System.Text.Json;
using RobloxCS.TypeGenerator.Models;
using Serilog;
using YamlDotNet.Core;
using YamlDotNet.Serialization;
using YamlDotNet.Serialization.NamingConventions;

namespace RobloxCS.TypeGenerator;

internal sealed class CreatorDocs {
    private const string RawRoot = "https://raw.githubusercontent.com/Roblox/creator-docs/main/content/en-us/reference/engine/";
    private const string DataTypeListUrl = "https://api.github.com/repos/Roblox/creator-docs/contents/content/en-us/reference/engine/datatypes";
    private static readonly TimeSpan CacheLifetime = TimeSpan.FromDays(1);

    private static readonly IDeserializer Yaml = new DeserializerBuilder()
        .WithNamingConvention(CamelCaseNamingConvention.Instance)
        .IgnoreUnmatchedProperties()
        .Build();

    private static readonly IDeserializer DataTypeYaml = new DeserializerBuilder()
        .WithNamingConvention(UnderscoredNamingConvention.Instance)
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
            var text = await ReadAsync(http, $"{RawRoot}classes/{name}.yaml", Path.Combine(cacheDirectory, $"{name}.yaml"), cancellation);
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

    public static async Task<Dictionary<string, DocDataType>> LoadDataTypesAsync(IEnumerable<string> fallbackNames, string cacheDirectory) {
        var directory = Path.Combine(cacheDirectory, "datatypes");
        Directory.CreateDirectory(directory);

        using var http = new HttpClient();
        http.DefaultRequestHeaders.UserAgent.ParseAdd("RobloxCS.TypeGenerator");

        var names = await ListDataTypesAsync(http, directory) ?? [.. fallbackNames.Distinct()];
        var dataTypes = new ConcurrentDictionary<string, DocDataType>();

        await Parallel.ForEachAsync(names, new ParallelOptions { MaxDegreeOfParallelism = 16 }, async (name, cancellation) => {
            var text = await ReadAsync(http, $"{RawRoot}datatypes/{name}.yaml", Path.Combine(directory, $"{name}.yaml"), cancellation);
            if (text is null) {
                return;
            }

            try {
                var doc = DataTypeYaml.Deserialize<DocDataType>(text);
                dataTypes[doc.Name.Length > 0 ? doc.Name : name] = doc;
            } catch (YamlException ex) {
                Log.Warning("Could not parse creator docs for datatype {DataType}: {Message}", name, ex.Message);
            }
        });

        Log.Information("Loaded creator docs for {Count} datatypes", dataTypes.Count);

        return new Dictionary<string, DocDataType>(dataTypes);
    }

    private static async Task<List<string>?> ListDataTypesAsync(HttpClient http, string directory) {
        var path = Path.Combine(directory, "index.txt");

        if (File.Exists(path) && DateTime.UtcNow - File.GetLastWriteTimeUtc(path) < CacheLifetime) {
            return [.. await File.ReadAllLinesAsync(path)];
        }

        try {
            using var response = await http.GetAsync(DataTypeListUrl);
            if (!response.IsSuccessStatusCode) {
                Log.Warning("Could not list creator docs datatypes ({Status}), falling back to names from the API dump", (int)response.StatusCode);

                return null;
            }

            using var json = JsonDocument.Parse(await response.Content.ReadAsStringAsync());

            var names = json.RootElement
                .EnumerateArray()
                .Select(entry => entry.GetProperty("name").GetString())
                .OfType<string>()
                .Where(name => name.EndsWith(".yaml", StringComparison.Ordinal))
                .Select(name => name[..^".yaml".Length])
                .ToList();

            await File.WriteAllLinesAsync(path, names);

            return names;
        } catch (Exception ex) when (ex is HttpRequestException or JsonException) {
            Log.Warning("Could not list creator docs datatypes ({Message}), falling back to names from the API dump", ex.Message);

            return null;
        }
    }

    private static async Task<string?> ReadAsync(HttpClient http, string url, string path, CancellationToken cancellation) {
        if (File.Exists(path) && DateTime.UtcNow - File.GetLastWriteTimeUtc(path) < CacheLifetime) {
            return await File.ReadAllTextAsync(path, cancellation);
        }

        using var response = await http.GetAsync(url, cancellation);
        if (!response.IsSuccessStatusCode) {
            Log.Verbose("No creator docs at {Url} ({Status})", url, (int)response.StatusCode);

            return null;
        }

        var text = await response.Content.ReadAsStringAsync(cancellation);
        await File.WriteAllTextAsync(path, text, cancellation);

        return text;
    }
}