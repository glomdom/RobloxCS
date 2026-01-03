#nullable enable
namespace RobloxCS.Types;
[RobloxNative("GlobalDataStore", RobloxNativeType.Instance)]
public partial class GlobalDataStore : Instance  {
    public LuaTuple GetAsync() => throw new InvalidOperationException("Cannot call reserved method for RobloxCS transpiler.");
    public object IncrementAsync() => throw new InvalidOperationException("Cannot call reserved method for RobloxCS transpiler.");
    public LuaTuple RemoveAsync() => throw new InvalidOperationException("Cannot call reserved method for RobloxCS transpiler.");
    public object SetAsync() => throw new InvalidOperationException("Cannot call reserved method for RobloxCS transpiler.");
    public LuaTuple UpdateAsync() => throw new InvalidOperationException("Cannot call reserved method for RobloxCS transpiler.");
}
