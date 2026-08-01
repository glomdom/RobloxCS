namespace RobloxCS.Common.Diagnostics;

/// <summary>
/// Contains diagnostic IDs for every possible diagnostic.
///
/// <c>0001-0099</c> for pipeline issues, <c>0100-9999</c> for compiler issues.
/// </summary>
public static class DiagnosticId {
    public const string NoSolutionFound = "RBXCS0001";
    public const string SolutionLoadFailed = "RBXCS0002";
    public const string FileNotFound = "RBXCS0003";
    public const string InvalidProjectJson = "RBXCS0004";
    public const string FailedToGetCompilation = "RBXCS0005";
    public const string NoMetadataReferences = "RBXCS0006";
    public const string NoAnchorFound = "RBXCS0007";
    public const string NoSyntaxTree = "RBXCS0008";
    public const string SourceDidNotCompile = "RBXCS0009";
}