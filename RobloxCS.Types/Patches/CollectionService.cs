namespace RobloxCS.Types;

public partial class CollectionService {
    public RBXScriptSignal<T> GetInstanceAddedSignal<T>(string tag) where T : Instance => ThrowHelper.ThrowTranspiledMethod();
    public RBXScriptSignal<T> GetInstanceRemovedSignal<T>(string tag) where T : Instance => ThrowHelper.ThrowTranspiledMethod();
}