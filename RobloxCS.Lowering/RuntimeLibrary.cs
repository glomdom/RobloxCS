namespace RobloxCS.Lowering;

public static class RuntimeLibrary {
    public const string CollectionsRequirePath = "@game/ReplicatedStorage/rbxcs_include/Collections";

    private static readonly Lazy<string> CollectionsCode = new(ReadCollections);

    public static void Install(string directory) {
        var code = CollectionsCode.Value;
        var path = Path.Join(directory, "Collections.luau");
        if (File.Exists(path) && File.ReadAllText(path) == code) {
            return;
        }

        Directory.CreateDirectory(directory);
        File.WriteAllText(path, code);
    }

    private static string ReadCollections() {
        using var stream = typeof(RuntimeLibrary).Assembly.GetManifestResourceStream("RobloxCS.RuntimeLib.Collections.luau")
                           ?? throw new InvalidOperationException("The collections runtime is missing from the compiler.");

        using var reader = new StreamReader(stream);

        return reader.ReadToEnd();
    }
}
