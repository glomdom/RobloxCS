#nullable enable
namespace RobloxCS.Types;
[RobloxNative("TextGenerator", RobloxNativeType.Instance)]
public partial class TextGenerator : Instance  {
    public int Seed { get; } = default!;
    public string SystemPrompt { get; } = default!;
    public float Temperature { get; } = default!;
    public float TopP { get; } = default!;
    public Dictionary<string, object> GenerateTextAsync() => throw new InvalidOperationException("Cannot call reserved method for RobloxCS transpiler.");
}
