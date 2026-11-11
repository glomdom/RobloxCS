using RobloxCS.Types.Attributes;

namespace RobloxCS.Tests.Data;

internal class PrimaryCounter(int current, string label = "counter") {
    private int Initial = current;

    public int Snapshot { get; } = current;
    public string Label => label;

    public int Value {
        get => current;
        set => current = value;
    }

    public void Add(int amount) {
        current += amount;
    }

    public void Increment() {
        current++;
    }

    public int Original() {
        return Initial;
    }

    public int Shadow(int current) {
        return current;
    }
}

internal class PrimaryInitializerOnly(int value) {
    public int Field = value + 1;
    public int Property { get; } = value * 2;
}

internal class PrimaryUnused(int ignored) {}

internal class PrimaryEmpty() {
    public int Value = 7;
}

internal class PrimaryNames(int self, int @end) {
    public int Read() {
        return self + @end;
    }

    public void Update(int self) {
        @end = self;
    }
}

internal class PrimaryConstructors {
    private static int Counter;

    private static int Next(string label) {
        Counter++;
        Console.WriteLine(label);

        return Counter;
    }

    [EntryPoint]
    internal static void Run() {
        var first = new PrimaryCounter(10);
        var second = new PrimaryCounter(label: "second", current: Next("argument"));

        Console.WriteLine(first.Label);
        Console.WriteLine(second.Label);
        Console.WriteLine(first.Value);
        Console.WriteLine(second.Value);

        first.Add(5);
        first.Increment();
        Console.WriteLine(first.Value);
        Console.WriteLine(first.Original());
        Console.WriteLine(first.Snapshot);
        Console.WriteLine(second.Value);

        first.Value = 30;
        Console.WriteLine(first.Value);
        Console.WriteLine(first.Shadow(99));
        Console.WriteLine(first.Value);

        var initializer = new PrimaryInitializerOnly(4);
        Console.WriteLine(initializer.Field);
        Console.WriteLine(initializer.Property);
        Console.WriteLine(new PrimaryEmpty().Value);

        var unused = new PrimaryUnused(Next("unused"));
        Console.WriteLine(unused != null);

        var names = new PrimaryNames(@end: Next("end"), self: Next("self"));
        Console.WriteLine(names.Read());
        names.Update(20);
        Console.WriteLine(names.Read());
    }
}