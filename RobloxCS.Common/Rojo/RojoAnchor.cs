namespace RobloxCS.Common.Rojo;

public sealed record RojoAnchor(string FullPath, IReadOnlyList<string> InstancePath) {
    public override string ToString() => $"{FullPath} -> {string.Join('.', InstancePath)}";
}