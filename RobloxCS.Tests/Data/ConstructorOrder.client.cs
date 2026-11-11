using RobloxCS.Types.Attributes;

namespace RobloxCS.Tests.Data;

internal class ConstructorOrder {
    [EntryPoint]
    internal static void Run() {
        var item = new ConstructorOrderLeaf();

        Console.WriteLine(item.Read());
        Console.WriteLine(item.Extra);

        var other = new ConstructorOrderLeaf();
        item.Extra = 30;

        Console.WriteLine(other.Extra);
    }
}

internal class ConstructorOrderLeaf : ConstructorOrderBase {
    public int Extra = Init("derived field", 7);

    internal ConstructorOrderLeaf() {
        Console.WriteLine("derived body");
    }

    public override int Read() {
        return Extra;
    }
}

internal class ConstructorOrderBase {
    private int BaseValue = Init("base field", 4);

    protected static int Init(string label, int value) {
        Console.WriteLine(label);

        return value;
    }

    internal ConstructorOrderBase() {
        Console.WriteLine("base body:" + Read());
    }

    public virtual int Read() {
        return BaseValue;
    }
}