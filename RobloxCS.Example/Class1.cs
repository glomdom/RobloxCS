namespace RobloxCS.Example;

internal class Class1 {
    public int A = 0;
    public readonly string Name;

    public Class1(string name) {
        Name = name;
        A = 10;

        var xx = A + 5;
        
        {
            var xxShadow = xx * 2;
            xx = xxShadow;
        }
        //
        //     if (local > 10) {
        //         A = local;
        //     } else {
        //         A = -1;
        //     }
        //
        //     while (A < 20) {
        //         A++;
        //     }
        //
        //     for (var i = 0; i < 3; i++) {
        //         A += i;
        //     }
        // }
        //
        // public int DoubleA() {
        //     var result = A * 2;
        //
        //     return result;
        // }
        //
        // public void PrintName() {
        //     Console.WriteLine(Name);
        // }
    }
}