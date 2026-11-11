using RobloxCS.Types.Attributes;

namespace RobloxCS.Tests.Data;

internal class ObjectInitialization {
    public int Value;
    public string Label = "default";
    public bool Enabled = false;
    public int Count = 0;
    internal static int Created;

    internal ObjectInitialization() {
        Created++;

        Console.WriteLine("ctor");
    }

    internal static int Mark(int value) {
        Console.WriteLine("set:" + value);

        return value;
    }
}

internal class ObjectInitializationDriver {
    [EntryPoint]
    internal static void Run() {
        var first = new ObjectInitialization { Value = ObjectInitialization.Mark(10), Label = "first" };
        var second = new ObjectInitialization { Value = ObjectInitialization.Mark(20), Label = "second" };

        Console.WriteLine(first.Value);
        Console.WriteLine(first.Label);
        Console.WriteLine(second.Value);
        Console.WriteLine(second.Label);

        first.Value++;

        Console.WriteLine(second.Value);
        Console.WriteLine(ObjectInitialization.Created);
        Console.WriteLine(first.Enabled);
        Console.WriteLine(first.Count);
    }
}