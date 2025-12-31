namespace RobloxCS.TypeGenerator.Models;

public class RobloxEnum {
    public required string Name { get; init; }
    public required List<RobloxEnumItem> Items { get; init; }
    public List<RobloxTagKind>? Tags { get; init;}
}