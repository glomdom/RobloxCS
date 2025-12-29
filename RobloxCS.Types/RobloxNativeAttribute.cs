namespace RobloxCS.Types;

[AttributeUsage(AttributeTargets.Class | AttributeTargets.Struct)]
internal class RobloxNativeAttribute : Attribute {
    public string RobloxName { get; }
    public RobloxNativeType NativeType { get; }

    public RobloxNativeAttribute(string name, RobloxNativeType type) {
        RobloxName = name;
        NativeType = type;
    }
}