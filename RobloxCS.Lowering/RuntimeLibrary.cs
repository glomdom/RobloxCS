namespace RobloxCS.Lowering;

public static class RuntimeLibrary {
    public const string CollectionsRequirePath = "@game/ReplicatedStorage/Shared/__RobloxCS/Collections";
    public const string DirectoryName = "__RobloxCS";

    public static void InstallCollections(string directory) {
        using var stream = typeof(RuntimeLibrary).Assembly.GetManifestResourceStream("RobloxCS.RuntimeLib.Collections.luau")
                           ?? throw new InvalidOperationException("The collections runtime is missing from the compiler.");

        using var reader = new StreamReader(stream);

        var code = reader.ReadToEnd();
        var path = Path.Join(directory, "Collections.luau");
        if (File.Exists(path) && File.ReadAllText(path) == code) {
            return;
        }

        Directory.CreateDirectory(directory);
        File.WriteAllText(path, code);
    }
}