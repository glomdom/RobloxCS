using RobloxCS.Types.Attributes;

namespace RobloxCS.Tests.Data;

internal class PrimaryRoot(int value) {
    public int Initial { get; } = value;

    public virtual int Read() {
        return value;
    }

    public void ChangeRoot(int replacement) {
        value = replacement;
    }
}

internal class PrimaryMiddle(int value) : PrimaryRoot(value * 2) {
    public int Middle() {
        return value;
    }
}

internal class PrimaryLeaf(int value) : PrimaryMiddle(value + 1) {
    public override int Read() {
        return base.Read() + value;
    }

    public void ChangeLeaf(int replacement) {
        value = replacement;
    }
}

internal class PrimaryProbeBase {
    protected static int Trace(string label, int value) {
        Console.WriteLine(label + ":" + value);

        return value;
    }

    internal PrimaryProbeBase(int first, int second) {
        Console.WriteLine("base:" + first + ":" + second);
        Console.WriteLine(Read());
    }

    public virtual int Read() {
        return -1;
    }
}

internal class PrimaryProbe(int value) : PrimaryProbeBase(second: Trace("second", value + 2), first: Trace("first", value + 1)) {
    private int Initial = Trace("field", value);

    public override int Read() {
        value++;

        return value;
    }

    public int Snapshot() {
        return Initial;
    }
}

internal class PrimaryOptionalBase(int value = 12) {
    public int Read() {
        return value;
    }
}

internal class PrimaryImplicitBase() : PrimaryOptionalBase {}

internal class PrimaryForwardOnly(int value) : PrimaryOptionalBase(value) {}

internal class PrimaryObject() : object() {
    public int Read() {
        return 4;
    }
}

internal class PrimaryConstructorInheritance {
    [EntryPoint]
    internal static void Run() {
        var leaf = new PrimaryLeaf(3);
        PrimaryRoot root = leaf;

        Console.WriteLine(root.Read());
        Console.WriteLine(leaf.Middle());
        Console.WriteLine(leaf.Initial);

        root.ChangeRoot(20);
        leaf.ChangeLeaf(7);
        Console.WriteLine(root.Read());
        Console.WriteLine(leaf.Middle());
        Console.WriteLine(leaf.Initial);

        var probe = new PrimaryProbe(5);
        Console.WriteLine(probe.Read());
        Console.WriteLine(probe.Snapshot());
        Console.WriteLine(new PrimaryImplicitBase().Read());
        Console.WriteLine(new PrimaryForwardOnly(21).Read());
        Console.WriteLine(new PrimaryObject().Read());
    }
}