namespace RobloxCS.Example;

internal class StressTest {
    private int counter = 0;
    private static int staticValue = 2;

    internal void Main() {
        for (int i = 0; i < 4; i++) {
            for (int j = 0; j < 3; j++) {
                if ((i + j) % 2 == 0) {
                    continue;
                }

                if (i == 2 && j == 1) {
                    break;
                }

                counter += i * j;
            }
        }

    //     int x = 0;
    //     do {
    //         if (x == 2) {
    //             x++;
    //             continue;
    //         }
    //
    //         counter += x;
    //         x++;
    //     } while (x < 5);
    //
    //     int y = 0;
    //     while (y < 3) {
    //         counter += Helper(y);
    //         y++;
    //     }
    //
    //     bool flag = (counter > 10) && !(counter % 2 == 0) ? true : false;
    //     if (flag) {
    //         counter += staticValue;
    //     } else {
    //         counter -= staticValue;
    //     }
    //
    //     if (false) { }
    //
    //     RandomMethod();
    // }
    //
    // private int Helper(int value) {
    //     return value * 2 + (staticValue - 1);
    // }
    //
    // private void RandomMethod() {
    //     for (int i = 0; i < counter; i++) {
    //         if (i > 15) break;
    //     }
    }
}