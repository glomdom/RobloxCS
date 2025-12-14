namespace RobloxCS.Tests.Data;

internal class ScopeAndState {
    private static int GlobalCounter = 0;
    private int ID = 0;

    internal void Main() {
        var instanceA = new ScopeAndState();
        var instanceB = new ScopeAndState();

        instanceA.SetID(10);
        instanceB.SetID(20);

        instanceA.IncrementGlobal();
        instanceB.IncrementGlobal();

        instanceA.PrintStatus(); // ID: 10 | Global: 2
        instanceB.PrintStatus(); // ID: 20 | Global: 2

        var ID = 999; 
        Console.WriteLine("Local Shadow ID: " + ID); 

        instanceA.PrintStatus(); 

        Console.WriteLine("--- End Scope Test ---");
    }

    private void SetID(int val) {
        ID = val;
    }

    private void IncrementGlobal() {
        GlobalCounter++;
    }

    private void PrintStatus() {
        Console.WriteLine("ID: " + ID);
        Console.WriteLine("Global: " + GlobalCounter);
    }
}