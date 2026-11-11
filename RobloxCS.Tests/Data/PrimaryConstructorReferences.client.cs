using RobloxCS.Types.Attributes;

namespace RobloxCS.Tests.Data;

internal interface IPrimaryReader {
    int Read();
}

internal class PrimaryReader(int value) : IPrimaryReader {
    public int Read() {
        return value;
    }

    public void Replace(int replacement) {
        value = replacement;
    }
}

internal class PrimaryConsumer(IPrimaryReader reader, string? label = null) {
    private IPrimaryReader Original = reader;

    public string Label => label ?? "missing";

    public int Read() {
        return reader.Read();
    }

    public int ReadOriginal() {
        return Original.Read();
    }

    public void Replace(IPrimaryReader replacement) {
        reader = replacement;
    }
}

internal class PrimaryConstructorReferences {
    [EntryPoint]
    internal static void Run() {
        var first = new PrimaryReader(4);
        var second = new PrimaryReader(9);
        var consumer = new PrimaryConsumer(first);
        var other = new PrimaryConsumer(second, "other");

        Console.WriteLine(consumer.Label);
        Console.WriteLine(other.Label);
        Console.WriteLine(consumer.Read());
        first.Replace(6);
        Console.WriteLine(consumer.Read());

        consumer.Replace(second);
        Console.WriteLine(consumer.Read());
        Console.WriteLine(consumer.ReadOriginal());
        Console.WriteLine(other.Read());
    }
}