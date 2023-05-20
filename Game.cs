namespace Advance
{
    // ! Currently using PieceColor from Piece.cs
    // public enum PlayerColor
    // {
    //     White,
    //     Black
    // }

    public class Game
    {
        internal static PieceColor PlayerColor { get; private set; }
        internal string SrcFilePath;
        internal string DestFilePath;
        internal Board Board { get; }

        public Game(string[] args)
        {
            // TODO: Validate args
            PlayerColor = args[0] == "white" ? PieceColor.White : PieceColor.Black;
            SrcFilePath = args[1];
            DestFilePath = args[2];

            string boardStr = FileIO.LoadFile(SrcFilePath);

            Board = new Board(boardStr);
            Moves.GetValidMoves(Board);
            MoveContent bestMove = Search.IterativeSearch(Board, 7);
            Console.WriteLine($"Moving {bestMove.MovingPiece.SrcPos} to {bestMove.MovingPiece.DestPos}");
            Board.MovePiece(Board, bestMove.MovingPiece.SrcPos, bestMove.MovingPiece.DestPos); // TODO: Make an overload that takes a MoveContent

            FileIO.SaveFile(DestFilePath, Board.ToString());
        }

        public override string ToString()
        {
            return $"Game:\n{Board}";
        }
    }
}