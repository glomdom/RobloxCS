using RobloxCS.Types.Attributes;

namespace RobloxCS.Tests.Data;

internal interface ICastReader {
    int Read();
}

internal interface ICastChild : ICastReader;

internal interface ICastMarker;

internal interface ICastOther {
    int Read();
}

internal class CastImplementation : ICastChild, ICastMarker, ICastOther {
    public int Read() {
        return 7;
    }

    int ICastOther.Read() {
        return 9;
    }
}

internal class CastInheritedImplementation : CastImplementation;

internal class CastImpostor {
    public int Read() {
        return 42;
    }
}

internal class SafeCastInterfaces {
    [EntryPoint]
    internal static void Run() {
        var item = new CastInheritedImplementation();
        object source = item;
        var reader = source as ICastReader;

        Console.WriteLine(reader?.Read() ?? -1);
        Console.WriteLine((source as ICastOther)?.Read() ?? -1);
        Console.WriteLine((source as ICastChild)?.Read() ?? -1);
        Console.WriteLine((reader as ICastMarker) != null);
        Console.WriteLine((reader as ICastOther)?.Read() ?? -1);
        Console.WriteLine((reader as CastImplementation) == item);
        Console.WriteLine((reader as CastInheritedImplementation) == item);

        object impostor = new CastImpostor();

        Console.WriteLine((impostor as ICastReader) == null);
        Console.WriteLine((impostor as ICastMarker) == null);
        Console.WriteLine(((object)"text" as ICastMarker) == null);
        Console.WriteLine(((object)5 as ICastMarker) == null);

        ICastMarker? missing = null;

        Console.WriteLine((missing as ICastReader) == null);
    }
}