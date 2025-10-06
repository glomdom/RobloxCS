namespace RobloxCS.Example;

internal class Program {
    internal void Main() {
        for (int i = 0; i < 5; i++) {
            if (i % 2 == 0) {
                continue;
            } else if (i == 3) {
                break;
            } else {
                while (false) {
                    // never runs
                }
            }
        }

        int j = 0;
        do {
            j++;
        } while (j < 3);
    }
}