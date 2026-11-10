using RobloxCS.Types.Attributes;

namespace RobloxCS.Tests.Data;

internal class InheritanceLeaf : InheritanceMiddle {
    private int Extra = 3;

    internal InheritanceLeaf(int value) : base(value + 1) {
        Console.WriteLine("leaf: " + Extra);
    }

    public override int GetValue() {
        return base.GetValue() + Extra;
    }

    public override int Computed => base.Computed + Extra;

    public override int Mutable {
        get => base.Mutable + 1;
        set => base.Mutable = value + 2;
    }

    public new int Read() {
        return 99;
    }
}

internal class InheritanceMiddle : InheritanceBase {
    internal InheritanceMiddle(int value) : base(value) { }

    public override int GetValue() {
        return base.GetValue() + 10;
    }
}

internal class InheritanceBase {
    protected int Value = 2;
    public int Auto { get; set; } = 4;

    internal InheritanceBase(int value) {
        Value = value;

        Console.WriteLine("base: " + GetValue());
    }

    public virtual int GetValue() {
        return Value;
    }

    public virtual int Computed => Value;
    public virtual int Mutable { get; set; } = 11;

    public int Read() {
        return GetValue();
    }
}

internal class InheritanceImplicit : InheritanceRoot {
    public new int Value = 9;

    public new virtual int Number() {
        return 20;
    }
}

internal abstract class InheritanceAbstract {
    public abstract int Result();
    public abstract int Score { get; }

    public int ReadResult() {
        return Result() + Score;
    }
}

internal class InheritanceConcrete : InheritanceAbstract {
    public override int Result() {
        return 12;
    }

    public override int Score => 5;
}

internal class InheritanceRoot {
    public int Value = 7;

    public virtual int Number() {
        return 10;
    }

    public int ReadNumber() {
        return Number();
    }
}

internal class InheritanceOptional : InheritanceOptionalBase;

internal class InheritanceOptionalBase {
    public int Value;

    internal InheritanceOptionalBase(int value = 15) {
        Value = value;
    }
}

internal class Inheritance {
    [EntryPoint]
    internal static void Run() {
        var leaf = new InheritanceLeaf(5);

        InheritanceBase parent = leaf;
        Console.WriteLine(parent.GetValue());
        Console.WriteLine(parent.Read());
        Console.WriteLine(leaf.Computed);
        Console.WriteLine(leaf.Auto);
        
        leaf.Auto = 8;
        
        Console.WriteLine(parent.Auto);
        Console.WriteLine(new InheritanceImplicit().Value);
        Console.WriteLine(leaf.Read());
        Console.WriteLine(parent.Mutable);
        
        parent.Mutable = 20;
        
        Console.WriteLine(leaf.Mutable);
        InheritanceAbstract concrete = new InheritanceConcrete();
        Console.WriteLine(concrete.ReadResult());
        
        var another = new InheritanceImplicit();
        InheritanceRoot root = another;
        
        Console.WriteLine(root.Value);
        Console.WriteLine(another.Number());
        Console.WriteLine(root.Number());
        Console.WriteLine(another.ReadNumber());
        
        another.Value = 42;
        
        Console.WriteLine(new InheritanceImplicit().Value);
        Console.WriteLine(new InheritanceOptional().Value);
    }
}