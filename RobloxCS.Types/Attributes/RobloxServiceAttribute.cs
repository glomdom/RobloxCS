namespace RobloxCS.Types;

[AttributeUsage(AttributeTargets.Property)]
internal sealed class RobloxServiceAttribute : Attribute {
    public string Name { get; }

    public RobloxServiceAttribute(string name) {
        Name = name;
    }
}