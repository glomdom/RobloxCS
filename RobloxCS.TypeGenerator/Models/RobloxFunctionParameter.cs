namespace RobloxCS.TypeGenerator.Models;

public class RobloxFunctionParameter {
    public required string Name { get; init; }
    public required RobloxType Type { get; init; }

    public override string ToString() => $"{Name}: {Type.Name}";
}