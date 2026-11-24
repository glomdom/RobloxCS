namespace RobloxCS.Lowering;

public static class RuntimeLibrary {
    public const string RequireFolder = "@game/ReplicatedStorage/rbxcs_include";

    public const string CollectionsModule = "Collections";
    public const string TypesModule = "Types";

    private static readonly string[] Modules = [CollectionsModule, TypesModule];

    private static readonly Lazy<Dictionary<string, string>> Code = new(() => Modules.ToDictionary(module => module, Read));

    public static void Install(string directory) {
        foreach (var (module, code) in Code.Value) {
            var path = Path.Join(directory, module + ".luau");
            if (File.Exists(path) && File.ReadAllText(path) == code) {
                continue;
            }

            Directory.CreateDirectory(directory);
            File.WriteAllText(path, code);
        }
    }

    private static string Read(string module) {
        using var stream = typeof(RuntimeLibrary).Assembly.GetManifestResourceStream($"RobloxCS.RuntimeLib.{module}.luau")
                           ?? throw new InvalidOperationException($"The {module} runtime is missing from the compiler.");

        using var reader = new StreamReader(stream);

        return reader.ReadToEnd();
    }
}
