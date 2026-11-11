using RobloxCS.Types.Attributes;

namespace RobloxCS.Tests.Data;

internal interface IMarker;

internal interface IAlsoMarker : IMarker;

internal class Marked : IAlsoMarker;

internal interface IOptionalCall {
    int Combine(int first, int second = 3);
}

internal class OptionalCall : IOptionalCall {
    public int Combine(int first, int second = 9) {
        return first * 10 + second;
    }
}

internal class InterfaceMarkers {
    private static IMarker Echo(IMarker value) {
        return value;
    }

    [EntryPoint]
    internal static void Run() {
        var item = new Marked();
        IMarker marker = item;

        Console.WriteLine(Echo(marker) == item);

        IAlsoMarker other = item;

        Console.WriteLine(marker == other);
        Console.WriteLine(marker != null);

        var call = new OptionalCall();
        IOptionalCall contract = call;

        Console.WriteLine(call.Combine(2));
        Console.WriteLine(contract.Combine(2));
        Console.WriteLine(contract.Combine(second: 8, first: 4));
    }
}