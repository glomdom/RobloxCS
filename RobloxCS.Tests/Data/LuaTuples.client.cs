using RobloxCS.Types;
using RobloxCS.Types.Attributes;

namespace RobloxCS.Tests.Data;

internal class LuaTuples {
    private static int Sum(LuaTuple values) {
        var total = 0;
        for (var i = 0; i < values.Count; i++) {
            total += values.Get<int>(i);
        }

        return total;
    }

    private static LuaTuple Make() {
        return new LuaTuple(4, "four", true);
    }

    [EntryPoint]
    internal static void Run() {
        var pair = new LuaTuple(1, "one");

        Console.WriteLine(pair.Count);
        Console.WriteLine(pair.Get<int>(0));
        Console.WriteLine(pair.Get<string>(1));
        Console.WriteLine(Sum(new LuaTuple(1, 2, 3)));
        Console.WriteLine(new LuaTuple().Count);

        Func<LuaTuple, LuaTuple> swap = values => new LuaTuple(values.Get<string>(1), values.Get<int>(0));
        var swapped = swap(pair);

        Console.WriteLine(swapped.Get<string>(0));
        Console.WriteLine(swapped.Get<int>(1));

        Func<int, LuaTuple, int> count = (offset, rest) => offset + rest.Count;

        Console.WriteLine(count(10, new LuaTuple(1, 2, 3, 4)));
        Console.WriteLine(count(10, swapped));

        Func<LuaTuple> passthrough = () => Make();
        var made = passthrough();

        Console.WriteLine(made.Count);

        var (first, second) = new LuaTuple("left", "right");

        Console.WriteLine((string)first);
        Console.WriteLine((string)second);

        object last = "unset";
        (_, last) = new LuaTuple("ignored", "kept");

        Console.WriteLine((string)last);
    }
}
