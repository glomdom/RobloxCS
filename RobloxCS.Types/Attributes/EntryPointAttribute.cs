namespace RobloxCS.Types.Attributes;

/// <summary>
/// Defines a method inside a class can be used as an entry point.
/// Only one per class is allowed.
/// </summary>
[AttributeUsage(AttributeTargets.Method)]
public sealed class EntryPointAttribute : Attribute;