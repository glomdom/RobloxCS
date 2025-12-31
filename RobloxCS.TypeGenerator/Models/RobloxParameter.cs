namespace RobloxCS.TypeGenerator.Models;

public class RobloxParameter {
    public required string Name { get; init; }
    public required RobloxType Type { get; init; }

    public override string ToString() => $"{Name}: {Type.Name}";
}