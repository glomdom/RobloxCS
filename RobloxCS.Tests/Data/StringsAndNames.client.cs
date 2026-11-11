using RobloxCS.Types.Attributes;

namespace RobloxCS.Tests.Data;

internal class StringsAndNames {
    [EntryPoint]
    internal static void Run() {
        var @end = "end";
        var @and = "and";
        var self = "self";
        var @type = "type";
        var print = "print";

        Console.WriteLine(@end + ":" + @and + ":" + self + ":" + @type + ":" + print);
        Console.WriteLine("quote: \" slash: \\ tab:\tend");
        Console.WriteLine("braces: {x} backtick: `");
        Console.WriteLine("line1\nline2");

        var number = 12;

        Console.WriteLine("value=" + (number + 3) + ", negative=" + -number);
        Console.WriteLine("" + number + number);
        Console.WriteLine(number + number + "");

        {
            var local = 5;
            Console.WriteLine(local);
        }

        {
            var local = 6;
            Console.WriteLine(local);
        }
    }
}