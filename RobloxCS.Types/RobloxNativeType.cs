namespace RobloxCS.Types;

internal enum RobloxNativeType {
    /// <summary>
    /// Corresponds to an <c>Instance</c> in Roblox.
    /// Generated code will take the form of <c>Instance.new(robloxName)</c>.
    /// </summary>
    Instance,

    /// <summary>
    /// Corresponds to a <c>Service</c> in Roblox.
    /// Generated code will take the form of <c>game:GetService(robloxName)</c>.
    /// </summary>
    Service,

    /// <summary>
    /// Corresponds to a datatype in Roblox.
    /// Generated code will take the form of <c>robloxName.new()</c>.
    /// </summary>
    DataType,
}