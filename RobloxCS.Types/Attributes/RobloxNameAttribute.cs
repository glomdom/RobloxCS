namespace RobloxCS.Types;

[AttributeUsage(AttributeTargets.Property | AttributeTargets.Method | AttributeTargets.Class)]
internal class RobloxNameAttribute : Attribute {
    public string RenameTo { get; set; }

    public RobloxNameAttribute(string renameTo) {
        RenameTo = renameTo;
    }
}