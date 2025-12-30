namespace RobloxCS.TypeGenerator.Models;

public class RobloxFunctionParameter {
    public required string Name { get; set; }
    public required RobloxType Type { get; set; }

    public override string ToString() => $"{Name}: {Type.Name}";
}