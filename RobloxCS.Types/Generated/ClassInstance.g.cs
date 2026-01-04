#nullable enable
namespace RobloxCS.Types;
[RobloxNative("Instance", RobloxNativeType.Instance)]
public partial class Instance : Object  {
    public bool Archivable { get; } = default!;
    public List<Enums.SecurityCapability> Capabilities { get; } = default!;
    public string Name { get; } = default!;
    public Instance Parent { get; } = default!;
    public Enums.PredictionMode PredictionMode { get; } = default!;
    public bool Sandboxed { get; } = default!;
    public void AddTag() => throw new InvalidOperationException("Cannot call reserved method for RobloxCS transpiler.");
    public void ClearAllChildren() => throw new InvalidOperationException("Cannot call reserved method for RobloxCS transpiler.");
    public Instance Clone() => throw new InvalidOperationException("Cannot call reserved method for RobloxCS transpiler.");
    public void Destroy() => throw new InvalidOperationException("Cannot call reserved method for RobloxCS transpiler.");
    public Instance FindFirstAncestor() => throw new InvalidOperationException("Cannot call reserved method for RobloxCS transpiler.");
    public Instance FindFirstAncestorOfClass() => throw new InvalidOperationException("Cannot call reserved method for RobloxCS transpiler.");
    public Instance FindFirstAncestorWhichIsA() => throw new InvalidOperationException("Cannot call reserved method for RobloxCS transpiler.");
    public Instance FindFirstChild() => throw new InvalidOperationException("Cannot call reserved method for RobloxCS transpiler.");
    public Instance FindFirstChildOfClass() => throw new InvalidOperationException("Cannot call reserved method for RobloxCS transpiler.");
    public Instance FindFirstChildWhichIsA() => throw new InvalidOperationException("Cannot call reserved method for RobloxCS transpiler.");
    public Instance FindFirstDescendant() => throw new InvalidOperationException("Cannot call reserved method for RobloxCS transpiler.");
    public Actor GetActor() => throw new InvalidOperationException("Cannot call reserved method for RobloxCS transpiler.");
    public object GetAttribute() => throw new InvalidOperationException("Cannot call reserved method for RobloxCS transpiler.");
    public RBXScriptSignal GetAttributeChangedSignal() => throw new InvalidOperationException("Cannot call reserved method for RobloxCS transpiler.");
    public Dictionary<string, object> GetAttributes() => throw new InvalidOperationException("Cannot call reserved method for RobloxCS transpiler.");
    public List<Instance> GetChildren() => throw new InvalidOperationException("Cannot call reserved method for RobloxCS transpiler.");
    public List<object> GetDescendants() => throw new InvalidOperationException("Cannot call reserved method for RobloxCS transpiler.");
    public string GetFullName() => throw new InvalidOperationException("Cannot call reserved method for RobloxCS transpiler.");
    public object GetStyled() => throw new InvalidOperationException("Cannot call reserved method for RobloxCS transpiler.");
    public RBXScriptSignal GetStyledPropertyChangedSignal() => throw new InvalidOperationException("Cannot call reserved method for RobloxCS transpiler.");
    public List<object> GetTags() => throw new InvalidOperationException("Cannot call reserved method for RobloxCS transpiler.");
    public bool HasTag() => throw new InvalidOperationException("Cannot call reserved method for RobloxCS transpiler.");
    public bool IsAncestorOf() => throw new InvalidOperationException("Cannot call reserved method for RobloxCS transpiler.");
    public bool IsDescendantOf() => throw new InvalidOperationException("Cannot call reserved method for RobloxCS transpiler.");
    public bool IsPropertyModified() => throw new InvalidOperationException("Cannot call reserved method for RobloxCS transpiler.");
    public List<Instance> QueryDescendants() => throw new InvalidOperationException("Cannot call reserved method for RobloxCS transpiler.");
    public void RemoveTag() => throw new InvalidOperationException("Cannot call reserved method for RobloxCS transpiler.");
    public void ResetPropertyToDefault() => throw new InvalidOperationException("Cannot call reserved method for RobloxCS transpiler.");
    public void SetAttribute() => throw new InvalidOperationException("Cannot call reserved method for RobloxCS transpiler.");
    public Instance WaitForChild() => throw new InvalidOperationException("Cannot call reserved method for RobloxCS transpiler.");
    public RBXScriptSignal<Instance, Instance> AncestryChanged { get; private set; } = null!;
    public RBXScriptSignal<string> AttributeChanged { get; private set; } = null!;
    public RBXScriptSignal<Instance> ChildAdded { get; private set; } = null!;
    public RBXScriptSignal<Instance> ChildRemoved { get; private set; } = null!;
    public RBXScriptSignal<Instance> DescendantAdded { get; private set; } = null!;
    public RBXScriptSignal<Instance> DescendantRemoving { get; private set; } = null!;
    public RBXScriptSignal Destroying { get; private set; } = null!;
    public RBXScriptSignal StyledPropertiesChanged { get; private set; } = null!;
}
