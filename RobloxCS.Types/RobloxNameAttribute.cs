namespace RobloxCS.Types;

[AttributeUsage(AttributeTargets.Property)]
internal class RobloxNameAttribute : Attribute {
    public string RenameTo { get; set; }

    public RobloxNameAttribute(string renameTo) {
        RenameTo = renameTo;
    }
}