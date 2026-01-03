#nullable enable
namespace RobloxCS.Types;
[RobloxNative("IncrementalPatchBuilder", RobloxNativeType.Service)]
public static partial class IncrementalPatchBuilderService {
    public static bool AddPathsToBundle { get; } = default!;
    public static double BuildDebouncePeriod { get; } = default!;
    public static bool HighCompression { get; } = default!;
    public static bool SerializePatch { get; } = default!;
    public static bool UseFileLevelCompressionInsteadOfChunk { get; } = default!;
    public static bool ZstdCompression { get; } = default!;
}
