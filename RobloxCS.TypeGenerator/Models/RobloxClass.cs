namespace RobloxCS.TypeGenerator.Models;

public class RobloxClass {
    public required string Name { get; init; }
    public required List<RobloxMember> Members { get; init; }
    public required string MemoryCategory { get; init; }
    public required string Superclass { get; init; }
    public List<RobloxTag>? Tags { get; init; }
}