#nullable enable
namespace RobloxCS.Types;
[RobloxNative("ContextActionService", RobloxNativeType.Service)]
public static partial class ContextActionService {
    public static void BindAction() => ThrowHelper.ThrowTranspiledMethod();
    public static void BindActionAtPriority() => ThrowHelper.ThrowTranspiledMethod();
    public static void BindActivate() => ThrowHelper.ThrowTranspiledMethod();
    public static Dictionary<string, object> GetAllBoundActionInfo() => ThrowHelper.ThrowTranspiledMethod();
    public static Dictionary<string, object> GetBoundActionInfo() => ThrowHelper.ThrowTranspiledMethod();
    public static string GetCurrentLocalToolIcon() => ThrowHelper.ThrowTranspiledMethod();
    public static void SetDescription() => ThrowHelper.ThrowTranspiledMethod();
    public static void SetImage() => ThrowHelper.ThrowTranspiledMethod();
    public static void SetPosition() => ThrowHelper.ThrowTranspiledMethod();
    public static void SetTitle() => ThrowHelper.ThrowTranspiledMethod();
    public static void UnbindAction() => ThrowHelper.ThrowTranspiledMethod();
    public static void UnbindActivate() => ThrowHelper.ThrowTranspiledMethod();
    public static void UnbindAllActions() => ThrowHelper.ThrowTranspiledMethod();
    public static Instance GetButton() => ThrowHelper.ThrowTranspiledMethod();
    public static RBXScriptSignal<Instance> LocalToolEquipped { get; private set; } = null!;
    public static RBXScriptSignal<Instance> LocalToolUnequipped { get; private set; } = null!;
}
