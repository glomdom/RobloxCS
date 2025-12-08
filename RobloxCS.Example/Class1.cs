namespace RobloxCS.Example;

internal class StressTest {
    private int Counter = 0;
    private static int StaticValue = 2;
    
    internal void Main() {
        for (var i = 0; i < 4; i++) {
            for (var j = 0; j < 3; j++) {
                if ((i + j) % 2 == 0) {
                    continue;
                }
        
                if (i == 2 && j == 1) {
                    break;
                }
        
                Counter += i * j;
            }
        }
    
        var x = 0;
        do {
            if (x == 2) {
                x++;
                continue;
            }
    
            Counter += x;
            x++;
        } while (x < 5);
    
        var y = 0;
        while (y < 3) {
            Counter += Helper(y);
            y++;
        }
    
        var flag = (Counter > 10) && Counter % 2 != 0;
        if (flag) {
            Counter += StaticValue;
        } else {
            Counter -= StaticValue;
        }
    
        if (false) { }
    
        RandomMethod();
    }
    
    private int Helper(int value) {
        return value * 2 + (StaticValue - 1);
    }
    
    private void RandomMethod() {
        for (var i = 0; i < Counter; i++) {
            if (i > 15) break;
        }
    }
}