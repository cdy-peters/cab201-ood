namespace Advance;

internal static class Program
{
    private static void Main(string[] args)
    {
        FileIO.ValidateArgs(args);
        if (args[0] == "name")
        {
            Console.WriteLine("SomeName");
            return;
        }

        Game game = new Game(player: args[0], srcFile: args[1], destFile: args[2]);

        // Console.WriteLine(game);
    }
}