using RobloxCS.Types.Attributes;

namespace RobloxCS.Tests.Data;

internal interface IParentContract {
    int Read();
}

internal interface ILeftContract : IParentContract {
    int Left();
}

internal interface IRightContract : IParentContract {
    int Right();
}

internal interface IDiamondContract : ILeftContract, IRightContract;

internal class ContractBase : IDiamondContract {
    public virtual int Read() {
        return 10;
    }

    public int Left() {
        return Read() + 1;
    }

    public int Right() {
        return Read() + 2;
    }
}

internal class ContractDerived : ContractBase {
    public override int Read() {
        return 20;
    }
}

internal class ContractHidden : ContractBase {
    public new int Read() {
        return 30;
    }
}

internal class ContractReimplemented : ContractBase, IParentContract {
    public new int Read() {
        return 40;
    }
}

internal abstract class ContractAbstract : IParentContract {
    public abstract int Read();
}

internal class ContractConcrete : ContractAbstract {
    public override int Read() {
        return 50;
    }
}

internal class InterfaceInheritance {
    [EntryPoint]
    internal static void Run() {
        IDiamondContract item = new ContractDerived();
        IParentContract parent = item;
        ILeftContract left = item;
        IRightContract right = item;

        Console.WriteLine(parent.Read());
        Console.WriteLine(left.Left());
        Console.WriteLine(right.Right());

        IParentContract hidden = new ContractHidden();

        Console.WriteLine(hidden.Read());
        Console.WriteLine(new ContractHidden().Read());

        IParentContract reimplemented = new ContractReimplemented();
        Console.WriteLine(reimplemented.Read());

        IParentContract concrete = new ContractConcrete();
        Console.WriteLine(concrete.Read());
    }
}