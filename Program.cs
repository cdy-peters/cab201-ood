namespace Advance;

public class Program
{
    private static void Main(string[] args)
    {
        Console.WriteLine("Advance");

        Game game = new Game(args);

        Console.WriteLine($"Initial Game = {game}");
    }
}