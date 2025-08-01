namespace RobloxCS.AST.Prefixes;

public sealed class NamePrefix : Prefix {
    public string Name { get; set; }

    public NamePrefix(string name) {
        Name = name;
    }
    
    public override string ToString() => Name;
}