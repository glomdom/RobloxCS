namespace RobloxCS.Example;

internal class Hero {
    public string Name;
    public int Health;

    public Hero(string name, int hp) {
        Name = name;
        Health = hp;
    }

    public void Heal(int amount) {
        Health += amount;
    }
}

internal class Program {
    internal void Main() {
        var h = new Hero("Noob", 100);

        Console.WriteLine("Hero: " + h.Name);

        h.Heal(50);

        Console.WriteLine("Health: " + h.Health);
    }
}