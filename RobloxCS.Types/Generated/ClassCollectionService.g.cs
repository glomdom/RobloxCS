#nullable enable
namespace RobloxCS.Types;
[RobloxNative("CollectionService", RobloxNativeType.Service)]
public static partial class CollectionService {
    public static void AddTag() => throw new InvalidOperationException("Cannot call reserved method for RobloxCS transpiler.");
    public static List<object> GetAllTags() => throw new InvalidOperationException("Cannot call reserved method for RobloxCS transpiler.");
    public static RBXScriptSignal GetInstanceAddedSignal() => throw new InvalidOperationException("Cannot call reserved method for RobloxCS transpiler.");
    public static RBXScriptSignal GetInstanceRemovedSignal() => throw new InvalidOperationException("Cannot call reserved method for RobloxCS transpiler.");
    public static List<Instance> GetTagged() => throw new InvalidOperationException("Cannot call reserved method for RobloxCS transpiler.");
    public static List<object> GetTags() => throw new InvalidOperationException("Cannot call reserved method for RobloxCS transpiler.");
    public static bool HasTag() => throw new InvalidOperationException("Cannot call reserved method for RobloxCS transpiler.");
    public static void RemoveTag() => throw new InvalidOperationException("Cannot call reserved method for RobloxCS transpiler.");
    public static RBXScriptSignal<string> TagAdded { get; private set; } = null!;
    public static RBXScriptSignal<string> TagRemoved { get; private set; } = null!;
}
