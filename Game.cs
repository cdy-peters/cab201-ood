namespace Advance
{
    public class Game
    {
        internal Board Board { get; }
        internal Moves Moves = new Moves();

        public Game()
        {
            string currFilepath = "tests/4/white/tests/0.txt";
            string text = System.IO.File.ReadAllText(currFilepath);

            // remove escape sequences
            text = text.Replace("\r", "");
            text = text.Replace("\n", "");

            Board = new Board(text);
            // Moves = new Moves();
            Moves.GetValidMoves(Board);
        }

        public override string ToString()
        {
            return $"Game:\n{Board}";
        }
    }
}