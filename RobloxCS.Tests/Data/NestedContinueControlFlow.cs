namespace RobloxCS.Tests.Data;

internal class NestedContinueControlFlow {
    internal void Main() {
        for (var x = 0; x < 3; x++) {
            for (var y = 0; y < 3; y++) {
                if (x == 1 && y == 1) {
                    Console.WriteLine("Skipping Center");

                    continue;
                }

                if (x == 2 && y == 2) {
                    Console.WriteLine("Breaking Inner");

                    break;
                }

                Console.WriteLine(x);
                Console.WriteLine(y);
            }
        }

        var count = 3;
        do {
            Console.WriteLine(count);

            count--;

            if (count == 1) {
                Console.WriteLine("Hit 1");
            }
        } while (count > 0);
    }
}