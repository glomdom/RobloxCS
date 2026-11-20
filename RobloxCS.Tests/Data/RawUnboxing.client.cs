using RobloxCS.Types;
using RobloxCS.Types.Attributes;

namespace RobloxCS.Tests.Data;

internal enum RawColor {
    Red,
    Green,
}

internal class RawUnboxing {
    [EntryPoint]
    internal static void Run() {
        var (count, ratio) = new LuaTuple(5, 2.5);

        Console.WriteLine((int)count + 1);
        Console.WriteLine((double)ratio * 2);
        Console.WriteLine(count as int? != null);
        Console.WriteLine(ratio as int? != null);
        Console.WriteLine(ratio as bool? != null);

        var flag = new LuaTuple(true).Get<object>(0);

        Console.WriteLine((bool)flag);
        Console.WriteLine(flag as bool? != null);

        var color = new LuaTuple(RawColor.Green).Get<object>(0);

        Console.WriteLine((RawColor)color);

        var maybe = count as int?;
        var missing = ratio as int?;

        Console.WriteLine(maybe ?? -1);
        Console.WriteLine(missing ?? -1);

        object boxed = 7;

        Console.WriteLine((int)boxed);
        Console.WriteLine(boxed as int? != null);
    }
}
