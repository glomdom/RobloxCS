#nullable enable
namespace RobloxCS.Types;
[RobloxNative("TextGenerator", RobloxNativeType.Instance)]
public partial class TextGenerator : Instance  {
    public int Seed { get; set; } = default!;
    public string SystemPrompt { get; set; } = default!;
    public float Temperature { get; set; } = default!;
    public float TopP { get; set; } = default!;
    public Dictionary<string, object> GenerateTextAsync() => throw new InvalidOperationException("Cannot call reserved method for RobloxCS transpiler.");
}
