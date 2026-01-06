#nullable enable
namespace RobloxCS.Types;
[RobloxNative("Selection", RobloxNativeType.Service)]
public static partial class SelectionService {
    public static float SelectionThickness { get; set; } = default!;
    public static RBXScriptSignal SelectionChanged { get; private set; } = null!;
}
