using RobloxCS.AST.Types;

namespace RobloxCS.Transpiler.Semantics;

public sealed class ClassInfo {
    public string Name { get; }
    public string? ParentName { get; }

    private readonly Dictionary<string, TypeInfo> _members = [];

    public ClassInfo(string name, string? parentName = null) {
        Name = name;
        ParentName = parentName;
    }

    public void AddMember(string name, TypeInfo type) => _members[name] = type;
    public TypeInfo? GetMember(string name) => _members.GetValueOrDefault(name);
}