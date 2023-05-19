namespace Advance
{
    public enum PlayerColor
    {
        White,
        Black
    }

    public class Game
    {
        internal PlayerColor PlayerColor;
        internal string SrcFilePath;
        internal string DestFilePath;
        internal Board Board { get; }
        internal Moves Moves = new Moves();

        public Game(string[] args)
        {
            // TODO: Validate args
            PlayerColor = args[0] == "white" ? PlayerColor.White : PlayerColor.Black;
            SrcFilePath = args[1];
            DestFilePath = args[2];

            string boardStr = FileIO.LoadFile(SrcFilePath);

            Board = new Board(boardStr);
            Moves.GetValidMoves(Board);
            Moves.FindMove(Board, PieceColor.White);

            FileIO.SaveFile(DestFilePath, Board.ToString());
        }

        public override string ToString()
        {
            return $"Game:\n{Board}";
        }
    }
}