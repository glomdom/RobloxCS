using RobloxCS.Types.Attributes;

namespace RobloxCS.Tests.Data;

internal interface IDispatch {
    int Apply(int value);
}

internal class DispatchAdd : IDispatch {
    private int Offset;

    internal DispatchAdd(int offset) {
        Offset = offset;
    }

    public int Apply(int value) {
        return value + Offset;
    }
}

internal class DispatchMultiply : IDispatch {
    public int Apply(int value) {
        return value * 3;
    }
}

internal class InterfaceDispatch {
    private static int Use(IDispatch action, int value) {
        return action.Apply(value);
    }

    private static IDispatch Make(bool add) {
        return add ? new DispatchAdd(10) : new DispatchMultiply();
    }

    [EntryPoint]
    internal static void Run() {
        IDispatch first = new DispatchAdd(2);
        IDispatch second = new DispatchMultiply();

        Console.WriteLine(first.Apply(4));
        Console.WriteLine(second.Apply(4));
        Console.WriteLine(Use(first, 9));
        Console.WriteLine(Make(true).Apply(1));
        Console.WriteLine(Make(false).Apply(2));

        IDispatch same = first;
        Console.WriteLine(same == first);
        Console.WriteLine(same == second);
    }
}