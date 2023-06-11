namespace Advance;

/// <summary>
/// The main program.
/// </summary>
internal static class Program
{
    /// <summary>
    /// The main entry point for the program.
    /// </summary>
    /// <param name="args">The command line arguments.</param>
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