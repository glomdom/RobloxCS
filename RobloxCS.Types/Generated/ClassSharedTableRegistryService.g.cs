#nullable enable
namespace RobloxCS.Types;
[RobloxNative("SharedTableRegistry", RobloxNativeType.Service)]
public static partial class SharedTableRegistryService {
    public static SharedTable GetSharedTable() => throw new InvalidOperationException("Cannot call reserved method for RobloxCS transpiler.");
    public static void SetSharedTable() => throw new InvalidOperationException("Cannot call reserved method for RobloxCS transpiler.");
}
