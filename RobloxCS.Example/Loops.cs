namespace RobloxCS.Example;

public class Loops {
    public readonly int A = 0;

    public Loops() {
        for (var i = 0; i < 10; i += 2) {
            A += i;
        }

        for (var i = 0; i < 3; i++) {
            for (var j = 0; j < 2; j++) {
                A += i * j;
            }
        }

        for (var i = 0; i < 3; i++) {
            while (i < 4) {
                i++;
            }
        }
    }
}