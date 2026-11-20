using RobloxCS.Types.Attributes;

namespace RobloxCS.Tests.Data;

internal class DefaultTarget(int value = 7, string label = "target") {
    internal int Value = value;
    internal string Label = label;
}

internal class DefaultArguments {
    private static int Total;

    private static void Add(int amount = 5) {
        Total += amount;
    }

    private static int Scale(int value, int factor = 3) {
        return value * factor;
    }

    private static string Describe(string name = "nobody", bool loud = false) {
        return loud ? name + "!" : name;
    }

    [EntryPoint]
    internal static void Run() {
        Add();
        Add(2);

        Console.WriteLine(Total);
        Console.WriteLine(Scale(2));
        Console.WriteLine(Scale(2, 4));
        Console.WriteLine(Describe());
        Console.WriteLine(Describe(loud: true));
        Console.WriteLine(Describe("someone"));

        var target = new DefaultTarget();
        Console.WriteLine(target.Value);
        Console.WriteLine(target.Label);
        Console.WriteLine(new DefaultTarget(label: "named").Value);
    }
}
